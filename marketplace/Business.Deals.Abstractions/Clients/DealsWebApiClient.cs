using Business.Abstractions.Auth;
using Business.Deals.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Deals.Abstractions.Clients
{
    public class DealsWebApiClient : BaseApiClientWithAuth<Deal>
    {
        public DealsWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<DealsWebApiClient> logger) 
            : base(httpClient, authStateProvider, configuration, logger) 
            => ResourceName = "deals";
    }
}
