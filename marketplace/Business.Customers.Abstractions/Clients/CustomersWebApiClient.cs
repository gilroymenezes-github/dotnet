using Business.Abstractions.Auth;
using Business.Customers.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Customers.Abstractions.Clients
{
    public class CustomersWebApiClient : BaseApiClientWithAuth<Customer>
    {
        public CustomersWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<CustomersWebApiClient> logger) 
            : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "customers";
        }
    }
}
