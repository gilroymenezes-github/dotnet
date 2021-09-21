using Business.Customers.Abstractions.Clients;
using Business.Customers.Abstractions.Models;
using Business.Db;
using Business.Projects.Abstractions.Clients;
using Business.Projects.Abstractions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Projects.BackgroundServices
{
    public class HarvestProjectsService : BackgroundService
    {
        private readonly ILogger<HarvestProjectsService> logger;
        private IEnumerable<Customer> customers;
        private ProjectsRepository dbRepository;
        private ProjectsHarvestApiClient harvestApiClient;
        private CustomersHttpClient customersHttpClient;
        private int pageCount;
        private int currentPage;
        private enum State { Initial, PageCountReceived, LastPageReceived, Done };
        private State currentState = State.Initial; 

        public HarvestProjectsService(
            ProjectsRepository dbRepository,
            ProjectsHarvestApiClient harvestApiClient,
            CustomersHttpClient customersHttpClient,
            ILogger<HarvestProjectsService> logger
            )
        {
            this.dbRepository = dbRepository;
            this.harvestApiClient = harvestApiClient;
            this.customersHttpClient = customersHttpClient;
            this.logger = logger;
            currentState = State.Done; // dont start anything till message from queue TBD
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);
                await GetHarvestProjects(); 
                logger.LogInformation($"Worker running at: {DateTimeOffset.Now} with processing page: {currentPage}");
            }
            await Task.CompletedTask;
        }

        private async Task GetHarvestProjects()
        {
            currentState = currentState switch
            {
                State.Initial => await Reset(),
                State.PageCountReceived => await ProcessProjectsFromPaging(),
                _ => State.Done
            };
        }

        private async Task<State> Reset()
        {
            await Task.CompletedTask;   // fake await
            pageCount = 26;             // this should be from api
            currentPage = 0;
            customers = await customersHttpClient.GetAsync();
            return customers.Count() == 0 ? State.Done : State.PageCountReceived;
        }

        private async Task<State> ProcessProjectsFromPaging()
        {
            var harvestProjects = await harvestApiClient.GetProjects(++currentPage);
            if (harvestProjects is null) return State.Done;
            var projects = new List<Project>();
            foreach (var harvestProject in harvestProjects)
            {
                var hp = harvestProject as IDictionary<string, object>;
                var projectId = hp["id"]?.ToString();
                var customerId = GetCustomerFromClientObject(hp["client"])?.Id;
                var project = new Project
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAtDateTimeUtc = DateTime.UtcNow,
                    UpdatedAtDateTimeUtc = DateTime.UtcNow,
                    ProjectBeginDateTimeUtc = new DateTime(2000, 1, 1),
                    ProjectEndDateTimeUtc = new DateTime(2000, 1, 1),
                    CustomerId = customerId,
                    Code = hp["code"]?.ToString(),
                    Name = hp["name"]?.ToString(),
                    BillBy = hp["bill_by"]?.ToString(),
                    IsActive = bool.Parse(hp["is_active"]?.ToString() ?? string.Empty),
                    IsBillable = bool.Parse(hp["is_billable"]?.ToString() ?? string.Empty),
                    ProjectId = projectId,
                };
                projects.Add(project);
            }

            projects.ForEach(async p =>
            {
                await dbRepository.CreateItemAsync(p, p.Id, p.CustomerId);
            });

            //// make n=5 buckets for bulk inserts
            //var split = projects.Count / 5;
            //var tasks = new List<Task>(5);
            //for (int i = 0; i < 5; ++i)
            //{
            //    tasks.Add(BulkInsertProjects(projects.Skip(split * i).Take(split)));
            //}
            //await Task.WhenAll(tasks);
            
            return currentPage == pageCount ? State.LastPageReceived : State.PageCountReceived;
        }

        private Customer GetCustomerFromClientObject(object o)
        {
            var clientItem = o as IDictionary<string, object>;
            var clientId = clientItem["id"]?.ToString();
            return customers.FirstOrDefault(c => c.CustomerId == clientId);
        }

        private async Task BulkInsertProjects(IEnumerable<Project> projects)
        {
            // concurrent tasks to create items 
            var tasks = new List<Task>(projects.Count());
            foreach (var project in projects)
            {
                tasks.Add(dbRepository.CreateItemAsync(project, project.Id)
                    .ContinueWith(itemResponse =>
                    {
                        if (!itemResponse.IsCompletedSuccessfully)
                        {
                            AggregateException innerExceptions = itemResponse.Exception.Flatten();
                            if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                            {
                                Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                            }
                            else
                            {
                                Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                            }
                        }
                    }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
