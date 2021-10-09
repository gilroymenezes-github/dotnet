using Azure.Messaging.ServiceBus;
using Business.Shared.Abstractions;
using Business.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Shared.Connections
{
    public class FilesQueueClient : BaseQueueClient<FileModel>
    {
        public FilesQueueClient(
            IConfiguration configuration, 
            ILogger<FilesQueueClient> logger
            ) : base(configuration, logger)
        {
            ResourceName = "files";
            Sender = Client.CreateSender(ResourceName);
        }

        public ServiceBusProcessor GetFilesMessageProcessor()
            => Client.CreateProcessor(ResourceName, new ServiceBusProcessorOptions());
    }
}
