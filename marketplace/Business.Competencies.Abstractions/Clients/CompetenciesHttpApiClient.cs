using Business.Abstractions;
using Business.Competencies.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Competencies.Abstractions.Clients
{
    public class CompetenciesHttpApiClient : BaseClient<Competency>
    {
        public CompetenciesHttpApiClient(
            HttpClient httpClient, 
            ILogger<CompetenciesHttpApiClient> logger
            ) : base(httpClient, logger)
        {
            ResourceName = "competencies";
        }

        public override async Task<IEnumerable<Competency>> GetAsync()
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-listing?code=J4iKa9bgFz0uRpeOZtSbpna5N9Qyz72sHCYfmxLgWkQUXX9gbadVpA==",
                GetStringContentFromObject(string.Empty));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(Competency[])
                : JsonConvert.DeserializeObject<Competency[]>(responseAsString);
        }

        public override async Task<Competency> GetFromIdAsync(string id)
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-single?code=KSH0KUFd218vuCz2X3GK0PRx96sPOmwMqqWdqoE9haUD5SnJhp4nkQ==",
                GetStringContentFromObject(new { id = id }));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(Competency)
                : JsonConvert.DeserializeObject<Competency[]>(responseAsString)[0];
        }

        public override async Task AddItemAsync(Competency item) => await Task.CompletedTask;

        public override async Task EditItemAsync(Competency item) => await Task.CompletedTask;

        public override async Task DeleteItemAsync(string id, bool isHardDelete) => await Task.CompletedTask;
    }
}
