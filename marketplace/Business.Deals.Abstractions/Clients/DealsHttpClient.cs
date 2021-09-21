using Business.Abstractions;
using Business.Deals.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Deals.Abstractions.Clients
{
    public class DealsHttpClient : BaseClient<Deal> 
    {
        public DealsHttpClient(
            HttpClient httpClient, 
            ILogger<DealsHttpClient> logger) 
            : base(httpClient, logger) 
        {
            ResourceName = "deals";
        }

        public override async Task<IEnumerable<Deal>> GetAsync()
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-listing?code=sLpWqOONN8wzoOUo9NC4CmaL35e0ZuRUbKIiOWt7wdabZwX9y5GQug==",
                GetStringContentFromObject(string.Empty));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Deal[]>(responseAsString);
        }

        public override async Task<Deal> GetFromIdAsync(string id)
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-single?code=6VwXIJacHySgwCcYLXBSNqHSm9J5ar0t/RpUjSH30YmkJPpYBiYFlw==", 
                GetStringContentFromObject(new { id = id }));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Deal[]>(responseAsString)[0];
        }

        public override async Task AddItemAsync(Deal item) => await Task.CompletedTask;

        public override async Task EditItemAsync(Deal item) => await Task.CompletedTask;

        public override async Task DeleteItemAsync(string id, bool isHardDelete = false) => await Task.CompletedTask;

    }
}
