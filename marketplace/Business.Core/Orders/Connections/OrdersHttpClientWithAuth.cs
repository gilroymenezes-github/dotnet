using Business.Shared.Auth;
using Business.Core.Orders.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Core.Orders.Connections
{
    public class OrdersHttpClientWithAuth : BaseHttpClientWithAuth<Order>
    {
        public OrdersHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<OrdersHttpClientWithAuth> logger)
            : base(httpClient, authStateProvider, configuration, logger)
            => ResourceName = "salesorders";
    }
}
