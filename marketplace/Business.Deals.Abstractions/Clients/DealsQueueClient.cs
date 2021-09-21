using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Deals.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Deals.Abstractions.Clients
{
    public class DealsQueueClient : BaseQueueClient<Deal>
    {
        public DealsQueueClient(IConfiguration configuration, ILogger<Deal> logger) 
            : base(configuration, logger)
        {
            resourceName = "deals";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetDealMessageProcessor() 
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
  
    }
}
