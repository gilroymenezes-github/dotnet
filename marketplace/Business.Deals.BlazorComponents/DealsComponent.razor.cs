using Business.Abstractions;
using Business.Deals.Abstractions.Clients;
using Business.Deals.Abstractions.Extensions;
using Business.Deals.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Deals.BlazorComponents
{
    public partial class DealsComponent : BaseComponent
    {
        [Inject] DealsWebApiClient DealsApiClient { get; set; }
        
        protected IEnumerable<Deal> deals;
        protected Deal currentDeal;
        protected string name;
        protected string desc;
                       
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnDeals");
        }

        protected async Task Load()
        {
            deals = await DealsApiClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddDeal()
        {
            var deal = new Deal { Name = name, Description = desc };
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            deal = deal.CreateFromDeal(email);
            await DealsApiClient.AddItemAsync(deal);
            Clear();
            await Load();
            NotificationService.Notify(NotificationSeverity.Success, "Deal Added");
        }

        protected async Task EditDeal()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var deal = currentDeal.UpdateFromDeal(email);
            await DealsApiClient.EditItemAsync(deal);
            Clear();
            await Load();
            NotificationService.Notify(NotificationSeverity.Success, "Deal Edited");
            //await hubConnection.SendAsync("SendMessage", "foo", DateTime.Now.ToLongTimeString());     
        }

        protected async Task DeleteDeal()
        {
            await DealsApiClient.DeleteItemAsync(currentDeal.Id);
            Clear();
            NotificationService.Notify(NotificationSeverity.Warning, "Deal Deleted");
            await Load();
        }

        protected async Task GetDeal(string id)
        {
            currentDeal = await DealsApiClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            name = string.Empty;
            desc = string.Empty;
            currentDeal = default(Deal);
            base.Clear();
        }
    }
}
