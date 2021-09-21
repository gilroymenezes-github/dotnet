using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Harvest.Projects
{
    public class HarvestClient
    {
        const string harvestId = "239876";
        const string harvestDeveloperToken = "1602045.pt.veF3cqkrn-dcj-ko7w_CX4XQXbq0jRwomksp9SVL9Xh9S7q2gjG-idaAiDHCep7sUPZLqI_GRT9smwlEovsKrw";

        public IEnumerable<string> GetColumnNames(IEnumerable<dynamic> records)
        {
            return (records.First() as IDictionary<string, object>).Keys;
        }

        public async Task<IList<dynamic>> GetUsers()
        {
            var harvestApiUrl = "https://api.harvestapp.com/v2/users";
            var data = await GetData(harvestApiUrl);
            var users = (data as IDictionary<string, object>)["users"] as IList<dynamic>;

            return users;
        }

        public async Task<IList<dynamic>> GetProjects()
        {
            var harvestApiUrl = "https://api.harvestapp.com/v2/projects";
            var data = await GetData(harvestApiUrl);
            var projects = (data as IDictionary<string, object>)["projects"] as IList<dynamic>;

            return projects;
        }

        private async Task<dynamic> GetData(string url)
            => await url
            .WithOAuthBearerToken(harvestDeveloperToken)
            .WithHeader("Harvest-Account-ID", harvestId)
            .WithHeader("User-Agent", "blazor")
            .GetJsonAsync();
    }
}
