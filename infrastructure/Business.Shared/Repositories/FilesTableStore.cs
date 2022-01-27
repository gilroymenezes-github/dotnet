using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
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
