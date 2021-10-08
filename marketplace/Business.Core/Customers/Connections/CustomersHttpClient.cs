using Business.Shared;
using Business.Core.Customers.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Shared.Abstractions;

namespace Business.Core.Customers.Connections
{
    public class CustomersHttpClient : BaseHttpClient<Customer>
    {
        public CustomersHttpClient(HttpClient httpClient, ILogger<CustomersHttpClient> logger) : base(httpClient, logger)
        {
            ResourceName = "customers";
        }

        public override Task AddItemAsync(Customer item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(Customer item) => Task.CompletedTask;
    }
}
