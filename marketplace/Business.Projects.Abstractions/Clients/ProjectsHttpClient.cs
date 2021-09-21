using Business.Abstractions;
using Business.Projects.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Projects.Abstractions.Clients
{
    public class ProjectsHttpClient : BaseClient<Project>
    {
        public ProjectsHttpClient(
            HttpClient httpClient,
            ILogger<ProjectsHttpClient> logger
            ) : base(httpClient, logger)
        {
            ResourceName = "projects";
        }

        public override Task AddItemAsync(Project item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(Project item) => Task.CompletedTask;

        public async Task<IEnumerable<Project>> GetFromCustomerIdAsync(string customerId)
        {
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/{ResourceName}/{customerId}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(IEnumerable<Project>)
                : JsonSerializer.Deserialize<IEnumerable<Project>>(responseAsString, JsonSerializerOptions);
        }

        public async Task<Project> GetFromIdAndCustomerIdAsync(string id, string customerId)
        {
            var customerIdParam = nameof(customerId);
            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/{ResourceName}/{id}?{customerIdParam}={customerId}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(Project)
                : JsonSerializer.Deserialize<Project>(responseAsString, JsonSerializerOptions);
        }
    }
}
