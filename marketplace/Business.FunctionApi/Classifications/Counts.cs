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
        readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase  };

        public class ChartModel
        {
            public List<DateTime> Labels { get; set; }
            public List<ChartModelItem> DataSets { get; set; }
        }

        public class ChartModelItem
        {
            public string Label { get; set; }
            public List<int> Data { get; set; }
        }

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
            var chartModel = await GetChartModel();
            var json = JsonSerializer.Serialize(chartModel, JsonSerializerOptions);
            response.WriteString(json);

            return response;
        }

        public async Task<ChartModel> GetChartModel()
        {
            var models = await countsStore.ReadItemsAsync("classifications");
            var groups = models.GroupBy(g => g.Name);
            var dates = new List<DateTime>();
            var resultDictionary = new Dictionary<string, Dictionary<DateTime, int>>();
            foreach (var group in groups)
            {
                if (!DateTime.TryParseExact(group.Key, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    continue;
                }
                var classificationDictionary = new Dictionary<string, int>() { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "G", 0 }, { "H", 0 } };
                group.ToList().ForEach(g =>
                {
                    var groupDictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(g.JsonData, JsonSerializerOptions);
                    foreach (var kvp in classificationDictionary)
                    {
                        classificationDictionary[kvp.Key] += groupDictionary[kvp.Key];
                    }
                });
                foreach(var kvp in classificationDictionary)
                {
                    if (!resultDictionary.Keys.Contains(kvp.Key)) resultDictionary.Add(kvp.Key, new Dictionary<DateTime, int>());
                    if (!resultDictionary[kvp.Key].ContainsKey(date)) resultDictionary[kvp.Key].Add(date, kvp.Value);
                    else resultDictionary[kvp.Key][date] = kvp.Value;
                }
                dates.Add(date);
            }
            var chartModel = new ChartModel();
            chartModel.Labels = dates;
            chartModel.DataSets = new List<ChartModelItem>();
            foreach(var kvp in resultDictionary)
            {
                chartModel.DataSets.Add(new ChartModelItem { Label = kvp.Key, Data = kvp.Value.Select(v => v.Value).ToList() });
            }
            return chartModel;
        }
    }
}
