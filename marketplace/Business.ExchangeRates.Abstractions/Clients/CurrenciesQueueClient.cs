using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class CurrenciesQueueClient : BaseQueueClient<Currency>
    {
        public CurrenciesQueueClient(IConfiguration configuration, ILogger<Currency> logger) 
            : base(configuration, logger)
        {
            resourceName = "currencies";
        }

        public ServiceBusProcessor GetCurrenciesMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
