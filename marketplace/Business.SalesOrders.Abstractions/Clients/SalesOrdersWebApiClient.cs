using Business.Abstractions.Auth;
using Business.SalesOrders.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.SalesOrders.Abstractions.Clients
{
    public class SalesOrdersWebApiClient : BaseApiClientWithAuth<SalesOrder>
    {
        public SalesOrdersWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<SalesOrdersWebApiClient> logger)
            : base(httpClient, authStateProvider, configuration, logger)
            => ResourceName = "salesorders";
    }
}
