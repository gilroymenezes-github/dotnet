using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Competencies.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Competencies.Abstractions.Clients
{
    public class CompetenciesQueueClient : BaseQueueClient<Competency>
    {
        public CompetenciesQueueClient(IConfiguration configuration, ILogger<Competency> logger)
            :base(configuration, logger)
        {
            resourceName = "competencies";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetCompetenciesMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
