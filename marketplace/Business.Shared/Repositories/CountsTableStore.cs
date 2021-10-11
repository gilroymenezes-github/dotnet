using Business.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Shared.Repositories
{
    public class CountsTableStore : AzureTableStorage<CountModel>
    {
        public CountsTableStore(
            IConfiguration configuration,
            ILogger<CountsTableStore> logger
            ) : base(configuration, logger)
        {
            ResourceName = "counts";
        }
    }
}
