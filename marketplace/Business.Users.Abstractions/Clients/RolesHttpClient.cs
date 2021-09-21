using Business.Abstractions.Auth;
using Business.Users.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Users.Abstractions.Clients
{
    public class RolesHttpClient : ValuesHttpClient<Role>
    {
        public RolesHttpClient(HttpClient httpClient, ILogger<Role> logger) 
            : base(httpClient, logger)
        {
            resourceName = "roles";
        }

        public async Task<IEnumerable<Role>> GetRolesAsync() => await GetValuesAsync();
    }
}
