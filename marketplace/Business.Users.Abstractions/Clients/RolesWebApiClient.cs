using Business.Abstractions.Auth;
using Business.Users.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Users.Abstractions.Clients
{
    public class RolesWebApiClient : ValuesApiClientWithAuth<Role>
    {
        private ILogger<RolesWebApiClient> logger;

        public RolesWebApiClient(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<RolesWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync() => await GetValues("roles");
    }
}
