using Azure.Messaging.ServiceBus;
using Infrastructure.Abstractions;
using Infrastructure.Constants;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Shared.Abstractions
{
    public abstract class BaseQueueClient<T> : IAsyncDisposable where T: BaseModel
    {
        private const string AzureServiceBusConnectionString = "Azure:ServiceBus:ConnectionString";
        protected IConfiguration configuration;
        protected ILogger logger;
        public ServiceBusClient Client { get; protected set; }
        public ServiceBusSender Sender { get; protected set; }
        public string ResourceName { get; protected set; }
        public HubConnection HubConnection { get; private set; }

        public async ValueTask DisposeAsync()
        {
            await Client.DisposeAsync();
        }

        public BaseQueueClient(IConfiguration configuration, ILogger<BaseQueueClient<T>> logger)
        {
            var connectionString = configuration.GetSection(AzureServiceBusConnectionString).Value;
            Client = new ServiceBusClient(connectionString);
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task CreateAsync(T item)
        {
            Sender ??= Client.CreateSender(ResourceName);
            var itemToAdd = new BaseCommand<T> { Command = CommandEnums.Add, Item = item };
            var itemAsJsonString = JsonSerializer.Serialize(itemToAdd);
            await SendItemMessageAsync(itemAsJsonString);
        }

        public async Task UpdateAsync(T item)
        {
            Sender ??= Client.CreateSender(ResourceName);
            var itemToEdit = new BaseCommand<T> { Command = CommandEnums.Edit, Item = item };
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
            => await Sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}
