using Business.Abstractions.Auth;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class ExchangeRatesWebApiClient : BaseApiClientWithAuth<ExchangeRate>
    {
        public ExchangeRatesWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<ExchangeRatesWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "exchangerates";
        }
    }
}
