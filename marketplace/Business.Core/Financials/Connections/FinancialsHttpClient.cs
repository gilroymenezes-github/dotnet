using Business.Shared;
using Business.Core.Financials.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Shared.Abstractions;

namespace Business.Core.Financials.Connections
{
    public class FinancialsHttpClient : BaseHttpClient<Financial> 
    {
        public FinancialsHttpClient(
            HttpClient httpClient, 
            ILogger<FinancialsHttpClient> logger) 
            : base(httpClient, logger) 
        {
            ResourceName = "deals";
        }

        public override async Task<IEnumerable<Financial>> GetAsync()
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-listing?code=sLpWqOONN8wzoOUo9NC4CmaL35e0ZuRUbKIiOWt7wdabZwX9y5GQug==",
                GetStringContentFromObject(string.Empty));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Financial[]>(responseAsString);
        }

        public override async Task<Financial> GetFromIdAsync(string id)
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-single?code=6VwXIJacHySgwCcYLXBSNqHSm9J5ar0t/RpUjSH30YmkJPpYBiYFlw==", 
                GetStringContentFromObject(new { id = id }));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Financial[]>(responseAsString)[0];
        }

        public override Task<Financial> AddItemAsync(Financial item) => (Task<Financial>)Task.CompletedTask;

        public override Task<Financial> EditItemAsync(Financial item) => (Task<Financial>)Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

    }
}
