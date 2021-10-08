using Business.Shared;
using Business.Core.Orders.Connections;
using Business.Core.Orders.Extensions;
using Business.Core.Orders.Models;
using Business.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Shared.Statics;

namespace Business.Core.WebApp.Components
{
    public partial class OrdersComponent : BaseComponent
    {
        [Inject] OrdersHttpClientWithAuth SalesOrdersApiClient { get; set; }
        
        protected IEnumerable<Order> orders;
        protected Order selectedOrder;
        protected string name;
        protected string desc;
        
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnSalesOrders");
        }

        protected async Task Load()
        {
            orders = await SalesOrdersApiClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddSalesOrder()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var salesOrder = new Order { Name = name, Description = desc };
            salesOrder = salesOrder.CreateFromSalesOrder(email);
            await SalesOrdersApiClient.AddItemAsync(salesOrder);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Order Added");
        }

        protected async Task EditSalesOrder()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var salesOrder = selectedOrder.UpdateFromSalesOrder(email);
            await SalesOrdersApiClient.EditItemAsync(salesOrder);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Order Edited");
            //await hubConnection.SendAsync("SendMessage", "foo", DateTime.Now.ToLongTimeString());
        }

        protected async Task DeleteSalesOrder()
        {
            await SalesOrdersApiClient.DeleteItemAsync(selectedOrder.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Order Deleted");
            await Load();
        }

        protected async Task GetSalesOrder(string id)
        {
            selectedOrder = await SalesOrdersApiClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            name = string.Empty;
            desc = string.Empty;
            selectedOrder = default(Order);
            base.Clear();
        }
    }

}
