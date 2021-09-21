using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Deals.Abstractions.Clients;
using Business.Deals.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Deals.BackgroundServices
{
    public class DealsService : BackgroundService
    {
        private readonly ILogger<DealsService> logger;
        private ServiceBusProcessor busProc;
        private DealsQueueClient sbClient;
        private DealsRepository dbRepository;

        public DealsService(ILogger<DealsService> logger, DealsQueueClient sbClient, DealsRepository dbRepository)
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.sbClient = sbClient;
            busProc = this.sbClient.GetDealMessageProcessor();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await sbClient.StartHubConnection(cancellationToken);
            busProc.ProcessMessageAsync += MessageHandler;
            busProc.ProcessErrorAsync += ErrorHandler;
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
                if (!busProc.IsProcessing)
                {
                    logger.LogInformation("Deals worker running at: {time}", DateTimeOffset.Now);
                    await busProc.StartProcessingAsync();
                }
            }
        }


        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            var itemCmd = System.Text.Json.JsonSerializer.Deserialize<DealCommand>(body);

            if (itemCmd.Command == CommandEnum.Add) await dbRepository.CreateItemAsync(itemCmd.Item, itemCmd.Item.Id);
            else if (itemCmd.Command == CommandEnum.Edit) await dbRepository.UpdateItemAsync(itemCmd.Item, itemCmd.Item.Id);

            await args.CompleteMessageAsync(args.Message);
            await sbClient.HubConnection.SendAsync(
                "SendMessageOnDeals", 
                "Deal Done", 
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
