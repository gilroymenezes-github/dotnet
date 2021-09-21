using Business.Abstractions.Auth;
using Business.Projects.Abstractions.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Projects.Abstractions
{
    public class ProjectsWebApiClient : BaseApiClientWithAuth<Project>
    {
        public ProjectsWebApiClient(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<ProjectsWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "projects";
        }

        public async Task<Project> GetProjectFromIdAndCustomerIdAsync(string id, string customerId)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            HttpClient.SetBearerToken(accessToken);

            var customerIdParam = nameof(customerId);
            var responseString = await HttpClient.GetStringAsync($"{HttpClient.BaseAddress}/{ResourceName}/{id}?{customerIdParam}={customerId}");
            return string.IsNullOrEmpty(responseString)
                ? default(Project)
                : JsonSerializer.Deserialize<Project>(responseString, JsonSerializerOptions);
        }

        public async Task<IEnumerable<Project>> GetProjectsFromCustomerIdAsync(string customerId)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            HttpClient.SetBearerToken(accessToken);

            var customerIdParam = nameof(customerId);
            var responseString = await HttpClient.GetStringAsync($"{HttpClient.BaseAddress}/{ResourceName}-customerid?{customerIdParam}={customerId}");
            return string.IsNullOrEmpty(responseString)
                ? default(IEnumerable<Project>)
                : JsonSerializer.Deserialize<IEnumerable<Project>>(responseString, JsonSerializerOptions);
        }
    }
}
