using Business.Abstractions.Auth;
using Business.Projections.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Projections.Abstractions.Clients
{
    public class ProjectionsWebApiClient : BaseApiClientWithAuth<Projection>
    {
        public ProjectionsWebApiClient(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<ProjectionsWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "projections";
        }
    }
}
