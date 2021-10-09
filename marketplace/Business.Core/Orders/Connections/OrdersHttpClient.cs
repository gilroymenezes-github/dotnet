using Business.Shared;
using Business.Core.Orders.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Shared.Abstractions;

namespace Business.Core.Orders.Connections
{
    public class OrdersHttpClient : BaseHttpClient<Order>
    {
        public OrdersHttpClient(
            HttpClient httpClient, 
            ILogger<OrdersHttpClient> logger) 
            : base(httpClient, logger)
        {
            ResourceName = "salesorders";
        }

        public override async Task<IEnumerable<Order>> GetAsync()
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-listing?code=yRpJuvDaQU/We2N7QLOK2Y5fE3VApDwPJmd0g65ar0By2fh0TQyj6w==",
                GetStringContentFromObject(string.Empty));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(Order[])
                : JsonConvert.DeserializeObject<Order[]>(responseAsString);
        }

        public override async Task<Order> GetFromIdAsync(string id)
        {
            var response = await HttpClient.PostAsync(
                HttpClient.BaseAddress + $"/{ResourceName}-single?code=x7qQ0R2vRH2vat1WxVn58dfCzzzEiEcVpripK4ta2CJM/mSUcuIAKg==",
                GetStringContentFromObject(new { id = id }));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString) 
                ? default(Order)
                : JsonConvert.DeserializeObject<Order[]>(responseAsString)[0];
        }

        public override Task<Order> AddItemAsync(Order item) => (Task<Order>)Task.CompletedTask;

        public override Task<Order> EditItemAsync(Order item) => (Task<Order>)Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;
       
    }
}
