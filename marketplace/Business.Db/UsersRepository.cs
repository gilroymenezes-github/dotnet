using Business.Db.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Business.Db
{
    public class UsersRepository : AzureStorageTableRepository<Users.Abstractions.Models.User> //CosmosDbRepository<Users.Abstractions.Models.User, ItemResponse<Users.Abstractions.Models.User>>
    {
        public UsersRepository(IConfiguration configuration, ILogger<UsersRepository> logger) 
            : base(configuration, logger)
        {
            ResourceName = "users";
        }
    }
}
