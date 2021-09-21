using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Projections.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Projections.Abstractions.Clients
{
    public class ProjectionsQueueClient : BaseQueueClient<Projection>
    {
        public ProjectionsQueueClient(
            IConfiguration configuration,
            ILogger<Projection> logger
            ) : base(configuration, logger)
        {
            resourceName = "projections";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetProjectionsMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
