using Business.Shared.Repositories;
using Business.Core.Financials.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Core.Financials.Repositories
{
    public class FinancialsRepository : AzureTableStorage<Financial>//CosmosDbRepository<Deal, ItemResponse<Deal>>
    {
        public FinancialsRepository(IConfiguration configuration, ILogger<FinancialsRepository> logger) 
            : base(configuration, logger) 
        {
            ResourceName = "Deals";
        }
    }
}
