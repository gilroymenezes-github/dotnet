using Business.Abstractions;
using Business.Customers.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Customers.Abstractions.Clients
{
    public class CustomersHttpClient : BaseClient<Customer>
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
