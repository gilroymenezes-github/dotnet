using Business.Db.Abstractions;
using Business.Deals.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Deals.BackgroundServices
{
    public class DealsRepository : AzureStorageTableRepository<Deal>//CosmosDbRepository<Deal, ItemResponse<Deal>>
    {
        public DealsRepository(IConfiguration configuration, ILogger<DealsRepository> logger) 
            : base(configuration, logger) 
        {
            ResourceName = "Deals";
        }
    }
}
