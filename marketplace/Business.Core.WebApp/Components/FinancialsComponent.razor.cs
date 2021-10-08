using Business.Shared;
using Business.Core.Financials.Connections;
using Business.Core.Financials.Extensions;
using Business.Core.Financials.Models;
using Business.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Shared.Statics;

namespace Business.Core.WebApp.Components
{
    public partial class FinancialsComponent : BaseComponent
    {
        [Inject] FinancialsHttpClientWithAuth FinancialsHttpClient { get; set; }
        
        protected IEnumerable<Financial> financials;
        protected Financial selectedFinancial;
        protected string name;
        protected string desc;
                       
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnDeals");
        }

        protected async Task Load()
        {
            financials = await FinancialsHttpClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddDeal()
        {
            var deal = new Financial { Name = name, Description = desc };
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            deal = deal.CreateFromDeal(email);
            await FinancialsHttpClient.AddItemAsync(deal);
            Clear();
            await Load();
            NotificationService.Notify(NotificationSeverity.Success, "Deal Added");
        }

        protected async Task EditDeal()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var deal = selectedFinancial.UpdateFromDeal(email);
            await FinancialsHttpClient.EditItemAsync(deal);
            Clear();
            await Load();
            NotificationService.Notify(NotificationSeverity.Success, "Deal Edited");
            //await hubConnection.SendAsync("SendMessage", "foo", DateTime.Now.ToLongTimeString());     
        }

        protected async Task DeleteDeal()
        {
            await FinancialsHttpClient.DeleteItemAsync(selectedFinancial.Id);
            Clear();
            NotificationService.Notify(NotificationSeverity.Warning, "Deal Deleted");
            await Load();
        }

        protected async Task GetDeal(string id)
        {
            selectedFinancial = await FinancialsHttpClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            name = string.Empty;
            desc = string.Empty;
            selectedFinancial = default(Financial);
            base.Clear();
        }
    }
}
