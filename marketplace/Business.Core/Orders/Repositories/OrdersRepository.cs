using Business.Shared.Repositories;
using Business.Core.Orders.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Core.Orders.Repositories
{
    public class OrdersRepository : AzureReadWriteTableStorage<Order>//CosmosDbRepository<SalesOrder, ItemResponse<SalesOrder>>
    {
        public OrdersRepository(IConfiguration configuration, ILogger<OrdersRepository> logger) 
            : base(configuration, logger)
        {
            ResourceName = "SalesOrders";
        }
    }
}
