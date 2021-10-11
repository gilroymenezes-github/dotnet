using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Shared.Models;
using Business.Shared.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Business.FunctionApi.Classifications
{
    public class Counts
    {
        readonly CountsTableStore countsStore;
        readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public Counts(CountsTableStore countsStore)
        {
            this.countsStore = countsStore;
        }

        [Function("Counts")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext
            )
        {
            var logger = executionContext.GetLogger("Counts");
            logger.LogInformation("C# HTTP trigger function processed counts.");

            //var response = req.CreateResponse(HttpStatusCode.Unauthorized);
            //if (!req.Headers.Contains("api-code"))
            //    return response;
            //if (!req.Headers.Any(h => h.Value.ToString() == "temp-api-code"))
            //    return response;

            var response = req.CreateResponse(HttpStatusCode.OK);
            var counts = await GetCounts();
            var json = JsonSerializer.Serialize(counts);
            response.WriteString(json);

            return response;
        }

        public async Task<IEnumerable<dynamic>> GetCounts()
        {
            var models = await countsStore.ReadItemsAsync("classifications");
            var groups = models.GroupBy(g => g.Name);
            var counts = new List<dynamic>();
            foreach (var group in groups)
            {
                var resultDictionary = new Dictionary<string, int>() { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "G", 0 }, { "H", 0 } };
                group.ToList().ForEach(g =>
                {
                    var groupDictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(g.JsonData, JsonSerializerOptions);
                    foreach (var kvp in resultDictionary)
                    {
                        resultDictionary[kvp.Key] += groupDictionary[kvp.Key];
                    }
                });
                if (DateTime.TryParseExact(group.Key, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    counts.Add(new { Date = date, Count = resultDictionary });
                }
            }
            return counts;
        }
    }
}
