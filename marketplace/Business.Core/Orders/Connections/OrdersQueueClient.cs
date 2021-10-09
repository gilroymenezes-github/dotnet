using Azure.Messaging.ServiceBus;
using Business.Core.Orders.Models;
using Business.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Core.Orders.Connections
{
    public class OrdersQueueClient : BaseQueueClient<Order>
    {
        public OrdersQueueClient(
            IConfiguration configuration, 
            ILogger<OrdersQueueClient> logger
            ) : base(configuration, logger)
        {
            ResourceName = "salesorders";
            Sender = Client.CreateSender(ResourceName);
        }

        public ServiceBusProcessor GetSalesOrderMessageProcessor()
            => Client.CreateProcessor(ResourceName, new ServiceBusProcessorOptions());
    }
}
