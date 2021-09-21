using Business.Abstractions;
using Business.SalesOrders.Abstractions.Clients;
using Business.SalesOrders.Abstractions.Extensions;
using Business.SalesOrders.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.SalesOrders.BlazorComponents
{
    public partial class SalesOrdersComponent : BaseComponent
    {
        [Inject] SalesOrdersWebApiClient SalesOrdersApiClient { get; set; }
        
        protected IEnumerable<SalesOrder> salesOrders;
        protected SalesOrder currentSalesOrder;
        protected string name;
        protected string desc;
        
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnSalesOrders");
        }

        protected async Task Load()
        {
            salesOrders = await SalesOrdersApiClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddSalesOrder()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var salesOrder = new SalesOrder { Name = name, Description = desc };
            salesOrder = salesOrder.CreateFromSalesOrder(email);
            await SalesOrdersApiClient.AddItemAsync(salesOrder);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Sales Order Added");
        }

        protected async Task EditSalesOrder()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var salesOrder = currentSalesOrder.UpdateFromSalesOrder(email);
            await SalesOrdersApiClient.EditItemAsync(salesOrder);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Sales Order Edited");
            //await hubConnection.SendAsync("SendMessage", "foo", DateTime.Now.ToLongTimeString());
        }

        protected async Task DeleteSalesOrder()
        {
            await SalesOrdersApiClient.DeleteItemAsync(currentSalesOrder.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Sales Order Deleted");
            await Load();
        }

        protected async Task GetSalesOrder(string id)
        {
            currentSalesOrder = await SalesOrdersApiClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            name = string.Empty;
            desc = string.Empty;
            currentSalesOrder = default(SalesOrder);
            base.Clear();
        }
    }

}
