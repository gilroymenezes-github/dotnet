using Microsoft.AspNetCore.SignalR;

namespace Business.Core.WebApi
{
    public class NotificationsHub : Hub
    {
        public void SendMessage(string name, string message) => Clients.All.SendAsync("BroadcastMessage", name, message);
       
        public void SendMessageOnDeals(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnDeals", name, message);

        public void SendMessageOnSalesOrders(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnSalesOrders", name, message);

        public void SendMessageOnCompetencies(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnCompetencies", name, message);

        public void SendMessageOnCurrencies(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnCurrencies", name, message);

        public void SendMessageOnExchangeRates(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnExchangeRates", name, message);

        public void SendMessageOnProjects(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnProjects", name, message);

        public void SendMessageOnCustomers(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnCustomers", name, message);

        public void SendMessageOnProjections(string name, string message) => Clients.All.SendAsync("BroadcastMessageOnProjections", name, message);
        

        public void Echo(string name, string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server) ");
        }
    }
}
