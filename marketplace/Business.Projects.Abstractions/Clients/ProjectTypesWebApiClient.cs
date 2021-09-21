using Business.Projects.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Abstractions.Auth
{
    public class ProjectTypesWebApiClient : ValuesApiClientWithAuth<ProjectType>
    {
        private ILogger<ProjectTypesWebApiClient> logger;
      
        public ProjectTypesWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<ProjectTypesWebApiClient> logger) 
            : base(httpClient, authStateProvider, configuration)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<ProjectType>> GetProjectTypesAsync() => await GetValues("projecttypes");
       
    }
}
