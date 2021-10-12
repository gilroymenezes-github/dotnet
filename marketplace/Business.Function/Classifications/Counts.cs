using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Function.Classifications
{
    public static class Counts
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        [FunctionName("classifications-counts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("counts", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "classifications");
            var query = new TableQuery<ClassificationCountModel>().Where(filter);
            var result = await cloudTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());
            var response = GetChartModel(result.Results);
            return new OkObjectResult(response);

        }

        public static ChartModel GetChartModel(IEnumerable<ClassificationCountModel> models)
        {
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
                foreach (var kvp in classificationDictionary)
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
            foreach (var kvp in resultDictionary)
            {
                chartModel.DataSets.Add(new ChartModelItem { Label = kvp.Key, Data = kvp.Value.Select(v => v.Value).ToList() });
            }
            return chartModel;
        }

        public class ClassificationCountModel : TableEntity
        {
            public string Name { get; set; }
            public string JsonData { get; set; }
        }

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
    }
}
