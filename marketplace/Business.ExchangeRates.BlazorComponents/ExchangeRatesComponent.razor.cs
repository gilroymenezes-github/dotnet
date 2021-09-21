using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Clients;
using Business.ExchangeRates.Abstractions.Extensions;
using Business.ExchangeRates.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.ExchangeRates.BlazorComponents
{
    public partial class ExchangeRatesComponent : BaseComponent
    {
        [Inject] ExchangeRatesWebApiClient ExchangeRatesApiClient { get; set; }
        [Inject] CurrenciesWebApiClient CurrenciesApiClient { get; set; }

        protected IEnumerable<ExchangeRate> ExchangeRates;
        protected IEnumerable<Currency> Currencies;
        protected ExchangeRate SelectedExchangeRate;
        protected Currency SelectedSourceCurrency;
        protected Currency SelectedTargetCurrency;
        protected decimal ResultConversionValue; 
        protected string ExchangeRateValueAsString;
        protected string WeightedFactorValueAsString;
        protected string SelectedSourceCurrencyCodeAsString { get => SelectedSourceCurrency.CurrencyCode; set => value = SelectedSourceCurrency.CurrencyCode; }
        

        protected override async Task OnInitializedAsync()
        {
            await Load();
            SelectedSourceCurrency ??= Currencies.FirstOrDefault();
            SelectedTargetCurrency ??= Currencies.FirstOrDefault(c => c.CurrencyCode == "INR");
            SelectedSourceCurrencyCodeAsString = SelectedSourceCurrency.CurrencyCode;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await StartHubConnection("BroadcastMessageOnExchangeRates");
        }

        protected async Task Load()
        {
            var taskExchangeRates = ExchangeRatesApiClient.GetAsync();
            var taskCurrencies = CurrenciesApiClient.GetAsync();
            await Task.WhenAll(taskExchangeRates, taskCurrencies);
            ExchangeRates = await taskExchangeRates;
            Currencies = await taskCurrencies;
        }

        protected void OnChange(object value)
        {
            SelectedSourceCurrency = Currencies.FirstOrDefault(c => c.Id == value.ToString());
            StateHasChanged();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddExchangeRate()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var exchangerate = new ExchangeRate
            {
                Name = $"{SelectedSourceCurrency.CurrencyCode}-{SelectedTargetCurrency.CurrencyCode}",
                ExchangeRateValue = decimal.Parse(ExchangeRateValueAsString),
                WeightedFactorValue = decimal.Parse(WeightedFactorValueAsString),
                SourceCurrencyId = SelectedSourceCurrency.CurrencyId,
                TargetCurrencyId = SelectedTargetCurrency.CurrencyId
            };
            exchangerate = exchangerate.CreateFromExchangeRate(email);
            await ExchangeRatesApiClient.AddItemAsync(exchangerate);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Exchange Rate Added");
        }

        protected async Task EditExchangeRate()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var exchangerate = SelectedExchangeRate;
            exchangerate = exchangerate.UpdateFromExchangeRate(email);
            await ExchangeRatesApiClient.EditItemAsync(exchangerate);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Exchange Rate Edited");
        }

        protected async Task DeleteExchangeRate()
        {
            await ExchangeRatesApiClient.DeleteItemAsync(SelectedExchangeRate.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Exchange Rate Deleted");
            await Load();
        }

        protected async Task GetExchangeRate(string id)
        {
            SelectedExchangeRate = await ExchangeRatesApiClient.GetFromIdAsync(id);
            ExchangeRateValueAsString = SelectedExchangeRate.ExchangeRateValue.ToString();
            WeightedFactorValueAsString = SelectedExchangeRate.WeightedFactorValue.ToString();
            SelectedSourceCurrency = Currencies.FirstOrDefault(c => c.CurrencyId == SelectedExchangeRate.SourceCurrencyId);
            SelectedTargetCurrency = Currencies.FirstOrDefault(c => c.CurrencyId == SelectedExchangeRate.TargetCurrencyId);
            SelectedSourceCurrencyCodeAsString = SelectedSourceCurrency.CurrencyCode;
            Mode = CommandEnum.Edit;
        }

        protected Currency GetCurrency(string id) => Currencies.FirstOrDefault(c => c.Id == id);

        protected decimal GetResultantConversion(decimal erValue, decimal wfValue) => erValue * wfValue;

        protected override void Clear()
        {
            ExchangeRateValueAsString = string.Empty;
            WeightedFactorValueAsString = string.Empty;
            SelectedSourceCurrency ??= Currencies.FirstOrDefault();
            SelectedTargetCurrency ??= Currencies.FirstOrDefault(c => c.CurrencyCode == "INR");
            SelectedExchangeRate = default(ExchangeRate);
            base.Clear();
        }
    }
}
