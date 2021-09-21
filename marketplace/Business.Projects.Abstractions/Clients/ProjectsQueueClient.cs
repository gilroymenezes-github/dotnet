using Azure.Messaging.ServiceBus;
using Business.Abstractions;
using Business.Projects.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Projects.Abstractions.Clients
{
    public class ProjectsQueueClient : BaseQueueClient<Project>
    {
        public ProjectsQueueClient(
            IConfiguration configuration,
            ILogger<Project> logger
            ) : base(configuration, logger)
        {
            resourceName = "projects";
            sender = client.CreateSender(resourceName);
        }

        public ServiceBusProcessor GetProjectsMessageProcessor()
            => client.CreateProcessor(resourceName, new ServiceBusProcessorOptions());
    }
}
