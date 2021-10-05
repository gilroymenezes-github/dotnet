using Business.Shared.Auth;
using Business.Core.Customers.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Core.Customers.Connections
{
    public class CustomersHttpClientWithAuth : BaseHttpClientWithAuth<Customer>
    {
        public CustomersHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<CustomersHttpClientWithAuth> logger) 
            : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "customers";
        }
    }
}
