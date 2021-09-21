using Azure.Messaging.ServiceBus;
using Business.Db;
using Business.Projects.Abstractions.Clients;
using Business.Projects.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Projects.BackgroundServices
{
    public class ProjectsService : BackgroundService
    {
        private readonly ILogger<ProjectsService> logger;
        private ServiceBusProcessor busProc;
        private ProjectsQueueClient sbClient;
        private ProjectsRepository dbRepository;

        public ProjectsService(
            ILogger<ProjectsService> logger,
            ProjectsQueueClient sbClient,
            ProjectsRepository dbRepository
            )
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.sbClient = sbClient;
            busProc = this.sbClient.GetProjectsMessageProcessor();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            busProc.ProcessMessageAsync += MessageHandler;
            busProc.ProcessErrorAsync += ErrorHandler;
            await sbClient.StartHubConnection(cancellationToken);
            await base.StartAsync(cancellationToken);
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            busProc.ProcessMessageAsync -= MessageHandler;
            busProc.ProcessErrorAsync -= ErrorHandler;
            await busProc.StopProcessingAsync();
            await sbClient.HubConnection.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(100, stoppingToken);
                await busProc.StartProcessingAsync();
                logger.LogInformation("Projects worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.CompletedTask;
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            var itemcmd = System.Text.Json.JsonSerializer.Deserialize<ProjectCommand>(body);

            itemcmd.Item.ProjectId = itemcmd.Item.Id;

            await dbRepository.UpdateItemAsync(itemcmd.Item, itemcmd.Item.Id);

            await args.CompleteMessageAsync(args.Message);
            await sbClient.HubConnection.SendAsync(
                "SendMessageOnProjects",
                "Projects Done",
                DateTime.Now.ToLongTimeString()
                );
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
