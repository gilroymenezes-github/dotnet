using Azure.Messaging.ServiceBus;
using Business.Shared.Connections;
using Business.Shared.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Process
{
    public class FilesProcessor : BackgroundService
    {
        private readonly ILogger<FilesProcessor> logger;
        private ServiceBusProcessor busProc;
        private FilesQueueClient qClient;
        private FilesTableStore dbClient;

        public FilesProcessor(
            FilesQueueClient qClient,
            FilesTableStore dbClient,
            ILogger<FilesProcessor> logger
            )
        {
            this.logger = logger;
            this.dbClient = dbClient;
            this.qClient = qClient;
            busProc = this.qClient.GetFilesMessageProcessor();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }

}
