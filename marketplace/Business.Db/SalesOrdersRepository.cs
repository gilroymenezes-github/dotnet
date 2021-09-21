using Business.Db.Abstractions;
using Business.SalesOrders.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.SalesOrders.BackgroundServices
{
    public class SalesOrdersRepository : AzureStorageTableRepository<SalesOrder>//CosmosDbRepository<SalesOrder, ItemResponse<SalesOrder>>
    {
        public SalesOrdersRepository(IConfiguration configuration, ILogger<SalesOrdersRepository> logger) 
            : base(configuration, logger)
        {
            ResourceName = "SalesOrders";
        }
    }
}
