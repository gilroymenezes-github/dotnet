using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Shared
{
    public abstract class BaseQueueClient<T> : IAsyncDisposable where T: BaseModel
    {
        private const string AzureServiceBusConnectionString = "Azure:ServiceBus:ConnectionString";
        protected IConfiguration configuration;
        protected ILogger logger;
        protected ServiceBusClient client;
        protected ServiceBusSender sender;
        protected string resourceName; // assigned by derived class

        public HubConnection HubConnection { get; private set; }

        public async ValueTask DisposeAsync()
        {
            await client.DisposeAsync();
        }

        public BaseQueueClient(IConfiguration configuration, ILogger<T> logger)
        {
            var connectionString = configuration.GetSection(AzureServiceBusConnectionString).Value;
            client = new ServiceBusClient(connectionString);
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task CreateAsync(T item)
        {
            sender ??= client.CreateSender(resourceName);
            var itemToAdd = new BaseCommand<T> { Command = CommandEnum.Add, Item = item };
            var itemAsJsonString = JsonSerializer.Serialize(itemToAdd);
            await SendItemMessageAsync(itemAsJsonString);
        }

        public async Task UpdateAsync(T item)
        {
            sender ??= client.CreateSender(resourceName);
            var itemToEdit = new BaseCommand<T> { Command = CommandEnum.Edit, Item = item };
            var itemAsJsonString = JsonSerializer.Serialize(itemToEdit);
            await SendItemMessageAsync(itemAsJsonString);
        }

        public async Task StartHubConnection(CancellationToken cancellationToken)
        {
            var hubConnectionString = configuration.GetSection("PowerUnitApi").Value;
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{hubConnectionString}/notifications")
                .Build();
            logger.LogInformation($"Started HubConnection With Url: {HubConnection.ToString()}");
            await HubConnection.StartAsync(cancellationToken);
        }

        private async Task SendItemMessageAsync(string message)
            => await sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}
