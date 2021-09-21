using Business.Customers.Abstractions.Models;
using Business.Db.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Db
{
    public class CustomersRepository : AzureStorageTableRepository<Customer> //CosmosDbRepository<Customer, ItemResponse<Customer>>
    {
        public CustomersRepository(IConfiguration configuration, ILogger<CustomersRepository> logger)
            : base(configuration, logger)
        {
            ResourceName = "customers";
        }
    }
}
