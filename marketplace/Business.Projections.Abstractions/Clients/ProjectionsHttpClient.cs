using Business.Abstractions;
using Business.Projections.Abstractions.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Projections.Abstractions.Clients
{
    public class ProjectionsHttpClient : BaseClient<Projection>
    {
        public ProjectionsHttpClient(
            HttpClient httpClient,
            ILogger<ProjectionsHttpClient> logger
            ) : base(httpClient, logger)
        {
            ResourceName = "projections";
        }

        public override Task AddItemAsync(Projection item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(Projection item) => Task.CompletedTask;

    }
}
