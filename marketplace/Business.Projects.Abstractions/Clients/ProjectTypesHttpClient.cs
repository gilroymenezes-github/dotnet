using Business.Abstractions.Auth;
using Business.Projects.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Projects.Abstractions.Clients
{
    public class ProjectTypesHttpClient : ValuesHttpClient<ProjectType>
    {
        public ProjectTypesHttpClient(
            HttpClient httpClient, 
            ILogger<ProjectType> logger) 
            : base(httpClient, logger)
        {
            resourceName = "projecttypes";
        }

        public async Task<IEnumerable<ProjectType>> GetProjectTypesAsync() => await GetValuesAsync();
    }
}
