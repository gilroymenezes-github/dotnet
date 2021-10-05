using Azure.Messaging.ServiceBus;
using Business.Shared;
using Business.Core.Financials.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Core.Financials.Connections
{
    public class FinancialsQueueClient : BaseQueueClient<Financial>
    {
        public FinancialsQueueClient(IConfiguration configuration, ILogger<Financial> logger) 
            : base(configuration, logger)
        {
            resourceName = "deals";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetDealMessageProcessor() 
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
  
    }
}
