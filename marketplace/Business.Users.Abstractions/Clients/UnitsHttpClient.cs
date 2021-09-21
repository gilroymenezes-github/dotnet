using Business.Abstractions.Auth;
using Business.Users.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Users.Abstractions.Clients
{
    public class UnitsHttpClient : ValuesHttpClient<Unit>
    {
        public UnitsHttpClient(HttpClient httpClient, ILogger<Unit> logger) 
            : base(httpClient, logger)
        {
            resourceName = "units";
        }

        public async Task<IEnumerable<Unit>> GetUnitsAsync() => await GetValuesAsync();
    }
}
