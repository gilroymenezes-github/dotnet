using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class CurrenciesHttpClient : BaseClient<Currency>
    {
        public CurrenciesHttpClient(
            HttpClient httpClient,
            ILogger<CurrenciesHttpClient> logger
            ) : base(httpClient, logger)
        {
            ResourceName = "currencies";
        }

        public override Task AddItemAsync(Currency item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(Currency item) => Task.CompletedTask;
       
    }
}
