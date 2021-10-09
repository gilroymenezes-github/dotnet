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
    public class FilesTableStore : AzureTableStorage<FileModel>
    {
        public FilesTableStore(
            IConfiguration configuration, 
            ILogger<FilesTableStore> logger)
            : base(configuration, logger) 
        {
            ResourceName = "files";
        }
    }
}
