using Business.Shared.Connections;
using Business.Core.Financials.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Business.Shared.Abstractions;

namespace Business.Core.Financials.Connections
{
    public class FinancialsHttpClientWithAuth : BaseHttpClientWithAuth<Financial>
    {
        public FinancialsHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<FinancialsHttpClientWithAuth> logger) 
            : base(httpClient, authStateProvider, configuration, logger) 
            => ResourceName = "deals";
    }
}
