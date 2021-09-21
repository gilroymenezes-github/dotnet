using Business.Db;
using Business.Users.Abstractions.Clients;
using Business.Users.Abstractions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Users.BackgroundServices
{
    public class HarvestUsersService : BackgroundService
    {
        private readonly ILogger<HarvestUsersService> logger;
        private UsersRepository dbRepository;
        private UsersHarvestApiClient harvestApiClient;
        private int pageCount;
        private int currentPage;
        private enum State { Initial, PageCountReceived, LastPageReceived, Done };
        private State currentState = State.Initial;

        public HarvestUsersService(
            UsersRepository dbRepository,
            UsersHarvestApiClient harvestApiClient,
            ILogger<HarvestUsersService> logger
            )
        {
            this.dbRepository = dbRepository;
            this.harvestApiClient = harvestApiClient;
            this.logger = logger;
            currentState = State.Done;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                await GetHarvestUsers();
                logger.LogInformation($"Worker running at: {DateTimeOffset.Now} with processing page: {currentPage}");
            }
            await Task.CompletedTask;
        }

        private async Task GetHarvestUsers()
        {
            currentState = currentState switch
            {
                State.Initial => await Reset(),
                State.PageCountReceived => await ProcessUsersFromPaging(),
                _ => State.Done
            };
        }

        private async Task<State> Reset()
        {
            await Task.CompletedTask;
            pageCount = 3;
            currentPage = 0;
            return State.PageCountReceived;
        }

        private async Task<State> ProcessUsersFromPaging()
        {
            var harvestUsers = await harvestApiClient.GetUsers(++currentPage);
            if (harvestUsers is null) return State.Done;
            var users = new List<Abstractions.Models.User>();
            foreach (var harvestUser in harvestUsers)
            {
                var hu = harvestUser as IDictionary<string, object>;
                var userId = hu["id"]?.ToString();
                var user = new Abstractions.Models.User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    CreatedAtDateTimeUtc = DateTime.UtcNow,
                    Email = hu["email"]?.ToString(),
                    Name = hu["first_name"]?.ToString() + " " + hu["last_name"]?.ToString(),
                    TimeZone = TimeZoneInfo.Local
                };
                users.Add(user);
            }
            // make n=5 buckets for bulk inserts
            var split = users.Count / 5;
            var tasks = new List<Task>(5);
            for (int i = 0; i < 5; ++i)
            {
                tasks.Add(BulkInsertProjects(users.Skip(split * i).Take(split)));
            }
            await Task.WhenAll(tasks);

            return currentPage == pageCount ? State.LastPageReceived : State.PageCountReceived;
        }

        private async Task BulkInsertProjects(IEnumerable<Abstractions.Models.User> users)
        {
            // concurrent tasks to create items 
            var tasks = new List<Task>(users.Count());
            foreach (var user in users)
            {
                tasks.Add(dbRepository.CreateItemAsync(user, user.Id)
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
