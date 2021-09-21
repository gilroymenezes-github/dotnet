using Business.Abstractions.Auth;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class CurrenciesWebApiClient : BaseApiClientWithAuth<Currency>
    {
        public CurrenciesWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration,
            ILogger<CurrenciesWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration, logger)

        {
            ResourceName = "currencies";
        }
    }
}
