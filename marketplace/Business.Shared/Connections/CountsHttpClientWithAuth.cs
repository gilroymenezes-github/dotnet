using Business.Shared.Abstractions;
using Business.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Shared.Connections
{
    public class CountsHttpClientWithAuth<T> : BaseHttpClientWithAuth<T> where T: CountModel
    {
        public CountsHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<BaseHttpClient<T>> logger
            ) : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "counts";
        }
    }
}
