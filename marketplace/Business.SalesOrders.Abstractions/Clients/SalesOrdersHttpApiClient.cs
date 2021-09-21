using Business.Abstractions;
using Business.SalesOrders.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.SalesOrders.Abstractions.Clients
{
    public class SalesOrdersHttpApiClient : BaseClient<SalesOrder>
    {
        public SalesOrdersHttpApiClient(
            HttpClient httpClient, 
            ILogger<SalesOrdersHttpApiClient> logger) 
            : base(httpClient, logger)
        {
            ResourceName = "salesorders";
        }

        public override async Task<IEnumerable<SalesOrder>> GetAsync()
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-listing?code=yRpJuvDaQU/We2N7QLOK2Y5fE3VApDwPJmd0g65ar0By2fh0TQyj6w==",
                GetStringContentFromObject(string.Empty));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(SalesOrder[])
                : JsonConvert.DeserializeObject<SalesOrder[]>(responseAsString);
        }

        public override async Task<SalesOrder> GetFromIdAsync(string id)
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-single?code=x7qQ0R2vRH2vat1WxVn58dfCzzzEiEcVpripK4ta2CJM/mSUcuIAKg==",
                GetStringContentFromObject(new { id = id }));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString) 
                ? default(SalesOrder)
                : JsonConvert.DeserializeObject<SalesOrder[]>(responseAsString)[0];
        }

        public override async Task AddItemAsync(SalesOrder item) => await Task.CompletedTask;

        public override async Task EditItemAsync(SalesOrder item) => await Task.CompletedTask;

        public override async Task DeleteItemAsync(string id, bool isHardDelete = false) => await Task.CompletedTask;
       
    }
}
