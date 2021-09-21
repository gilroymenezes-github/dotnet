using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class ExchangeRatesQueueClient : BaseQueueClient<ExchangeRate>
    {
        public ExchangeRatesQueueClient(IConfiguration configuration, ILogger<ExchangeRate> logger) 
            : base(configuration, logger)
        {
            resourceName = "exchangerates";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetExchangeRatesMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
