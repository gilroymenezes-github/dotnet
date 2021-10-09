using Business.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
