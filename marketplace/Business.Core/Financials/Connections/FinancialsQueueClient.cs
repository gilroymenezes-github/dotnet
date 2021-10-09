using Azure.Messaging.ServiceBus;
using Business.Shared;
using Business.Core.Financials.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Business.Shared.Abstractions;

namespace Business.Core.Financials.Connections
{
    public class FinancialsQueueClient : BaseQueueClient<Financial>
    {
        public FinancialsQueueClient(IConfiguration configuration, ILogger<FinancialsQueueClient> logger) 
            : base(configuration, logger)
        {
            ResourceName = "deals";
            Sender = Client.CreateSender(ResourceName);
        }

        public ServiceBusProcessor GetDealMessageProcessor() 
            => Client.CreateProcessor(ResourceName, new ServiceBusProcessorOptions());
  
    }
}
