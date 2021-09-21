using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Db;
using Business.Projections.Abstractions.Clients;
using Business.Projections.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Projections.BackgroundServices
{
    public class ProjectionsService : BackgroundService
    {
        private readonly ILogger<ProjectionsService> logger;
        private ServiceBusProcessor busProc;
        private ProjectionsQueueClient sbClient;
        
        public ProjectionsService(
            ProjectionsQueueClient sbClient,
            ILogger<ProjectionsService> logger
            
            )
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.sbClient = sbClient;
            busProc = this.sbClient.GetProjectionsMessageProcessor();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            busProc.ProcessMessageAsync += MessageHandler;
            busProc.ProcessErrorAsync += ErrorHandler;
            await sbClient.StartHubConnection(cancellationToken);
            await base.StartAsync(cancellationToken);
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancelleationToken)
        {
            busProc.ProcessMessageAsync -= MessageHandler;
            busProc.ProcessErrorAsync -= ErrorHandler;
            await busProc.StopProcessingAsync();
            await sbClient.HubConnection.StopAsync(cancelleationToken);
            await base.StopAsync(cancelleationToken);
            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                await busProc.StartProcessingAsync();
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.CompletedTask;
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            var itemcmd = System.Text.Json.JsonSerializer.Deserialize<ProjectionCommand>(body);

            itemcmd.Item.ProjectionId = itemcmd.Item.Id;

            if (itemcmd.Command == CommandEnum.Add)
                await dbRepository.CreateItemAsync(itemcmd.Item, itemcmd.Item.Id);
            else if (itemcmd.Command == CommandEnum.Edit)
                await dbRepository.UpdateItemAsync(itemcmd.Item, itemcmd.Item.Id);

            await args.CompleteMessageAsync(args.Message);
            await sbClient.HubConnection.SendAsync(
                "SendMessageOnProjections",
                "Projections Done",
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
