using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.WebApp.Chatbots.Pages
{
    public partial class Index : BaseComponent
    {
        private List<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task StartHubConnection(string broadcaster)
        {
            var hubConnectionString = Configuration.GetSection("PowerUnitApi").Value;
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{hubConnectionString}/notifications")
                .Build();
            HubConnection.On<string, string>($"{broadcaster}", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();

            });
            await HubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await StartHubConnection("BroadcastMessageOnChatbots");
        }

        async Task Send() => await HubConnection.SendAsync("SendMessageOnChatbots", userInput, messageInput);

        public bool IsConnected()
        {
            if (HubConnection is null) return false;
            return HubConnection.State == HubConnectionState.Connected
                ? true
                : false;
        }
            
    }
}
