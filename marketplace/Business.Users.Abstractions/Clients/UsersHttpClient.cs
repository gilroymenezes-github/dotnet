using Business.Abstractions;
using Business.Users.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Users.Abstractions.Clients
{
    public class UsersHttpClient : BaseClient<User>
    {
        public UsersHttpClient(HttpClient httpClient, ILogger<UsersHttpClient> logger)
            : base(httpClient, logger)
        {
            ResourceName = "users";
        }
        public override Task AddItemAsync(User item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(User item) => Task.CompletedTask;
    }
}
