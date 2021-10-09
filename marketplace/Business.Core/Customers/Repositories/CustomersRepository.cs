using Business.Core.Customers.Models;
using Business.Shared.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Core.Customers.Repositories
{
    public class CustomersRepository : AzureTableStorage<Customer> //CosmosDbRepository<Customer, ItemResponse<Customer>>
    {
        public CustomersRepository(IConfiguration configuration, ILogger<CustomersRepository> logger)
            : base(configuration, logger)
        {
            ResourceName = "customers";
        }
    }
}
