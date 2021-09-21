using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Competencies.Abstractions.Clients;
using Business.Competencies.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Competencies.BackgroundServices
{
    public class CompetenciesService : BackgroundService
    {
        private readonly ILogger<CompetenciesService> logger;
        private ServiceBusProcessor busProc;
        private CompetenciesQueueClient sbClient;
        private CompetenciesRepository dbRepository;

        public CompetenciesService(
            ILogger<CompetenciesService> logger, 
            CompetenciesQueueClient sbClient, 
            CompetenciesRepository dbRepository
            )
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.sbClient = sbClient;
            busProc = this.sbClient.GetCompetenciesMessageProcessor();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            busProc.ProcessMessageAsync += MessageHandler;
            busProc.ProcessErrorAsync += ErrorHandler;
            await sbClient.StartHubConnection(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            busProc.ProcessMessageAsync -= MessageHandler;
            busProc.ProcessErrorAsync -= ErrorHandler;
            await busProc.StopProcessingAsync();
            await sbClient.HubConnection.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
                await busProc.StartProcessingAsync();
                logger.LogInformation("Competencies worker running at: {time}", DateTimeOffset.Now);
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            var itemCmd = System.Text.Json.JsonSerializer.Deserialize<CompetencyCommand>(body);

            if (itemCmd.Command == CommandEnum.Add)
                await dbRepository.CreateItemAsync(itemCmd.Item, itemCmd.Item.Id);
            else if (itemCmd.Command == CommandEnum.Edit)
                await dbRepository.UpdateItemAsync(itemCmd.Item, itemCmd.Item.Id);

            await args.CompleteMessageAsync(args.Message);
            await sbClient.HubConnection.SendAsync(
                "SendMessageOnCompetencies", 
                "Competencies Done", 
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
