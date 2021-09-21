using Business.Abstractions.Auth;
using Business.Competencies.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Competencies.Abstractions.Clients
{
    public class CompetenciesWebApiClient : BaseApiClientWithAuth<Competency>
    {
        public CompetenciesWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<CompetenciesWebApiClient> logger)
            :base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "competencies";
        }
    }
}
