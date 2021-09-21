using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.SalesOrders.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.SalesOrders.Abstractions.Clients
{
    public class SalesOrdersQueueClient : BaseQueueClient<SalesOrder>
    {
        public SalesOrdersQueueClient(IConfiguration configuration, ILogger<SalesOrder> logger) 
            : base(configuration, logger)
        {
            resourceName = "salesorders";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetSalesOrderMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
