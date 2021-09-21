using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Clients;
using Business.ExchangeRates.Abstractions.Extensions;
using Business.ExchangeRates.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.ExchangeRates.BlazorComponents
{
    public partial class CurrenciesComponent : BaseComponent
    {
        [Inject] CurrenciesWebApiClient CurrenciesApiClient { get; set; }

        protected IEnumerable<Currency> Currencies;
        protected Currency SelectedCurrency;
        protected string CurrencyName;
        protected string CurrencyCode;

        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnCurrencies");
        }

        protected async Task Load()
        {
            Currencies = await CurrenciesApiClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddCurrency()
        {
            var currency = new Currency { Name = CurrencyName, CurrencyCode = CurrencyCode };
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            currency = currency.CreateFromCurrency(email);
            await CurrenciesApiClient.AddItemAsync(currency);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Currency Added");
        }

        protected async Task EditCurrency()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var currency = SelectedCurrency.UpdateFromCurrency(email);
            await CurrenciesApiClient.EditItemAsync(currency);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Currency Edited");
        }

        protected async Task DeleteCurrency()
        {
            await CurrenciesApiClient.DeleteItemAsync(SelectedCurrency.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Currency Deleted");
            await Load();
        }

        protected async Task GetCurrency(string id)
        {
            SelectedCurrency = await CurrenciesApiClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            CurrencyName = string.Empty;
            CurrencyCode = string.Empty;
            SelectedCurrency = default(Currency);
            base.Clear();
        }
    }
}
