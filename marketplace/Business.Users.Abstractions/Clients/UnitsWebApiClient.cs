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
    public class UnitsWebApiClient : ValuesApiClientWithAuth<Unit>
    {
        private ILogger<UnitsWebApiClient> logger;
     
        public UnitsWebApiClient(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<UnitsWebApiClient> logger
            ) : base(httpClient, authStateProvider, configuration)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<Unit>> GetUnits() => await GetValues("units");
    }
}
