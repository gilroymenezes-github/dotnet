using Azure.Messaging.ServiceBus;
using Business.Core.Orders.Connections;
using Business.Core.Orders.Models;
using Business.Core.Orders.Repositories;
using Business.Shared.Statics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Process
{
    public class OrdersProcessor : BackgroundService
    {
        private readonly ILogger<OrdersProcessor> logger;
        private ServiceBusProcessor busProc;
        private OrdersQueueClient sbClient;
        private OrdersRepository dbRepository;

        public OrdersProcessor(ILogger<OrdersProcessor> logger, 
            OrdersQueueClient sbClient, 
            OrdersRepository dbRepository)
        {
            this.logger = logger;
            this.dbRepository = dbRepository;
            this.sbClient = sbClient;
            busProc = this.sbClient.GetSalesOrderMessageProcessor();
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
                logger.LogInformation("SalesOrders worker running at: {time}", DateTimeOffset.Now);
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            logger.LogInformation($"Received: {body}");

            var itemCmd = System.Text.Json.JsonSerializer.Deserialize<OrderCommand>(body);

            if (itemCmd.Command == CommandEnum.Add) 
                await dbRepository.CreateItemAsync(itemCmd.Item, itemCmd.Item.Id);
            else if (itemCmd.Command == CommandEnum.Edit) 
                await dbRepository.UpdateItemAsync(itemCmd.Item, itemCmd.Item.Id);

            await args.CompleteMessageAsync(args.Message);
            await sbClient.HubConnection.SendAsync(
                "SendMessageOnSalesOrders", 
                "Sales Order Done", 
                DateTime.Now.ToLongTimeString());
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
