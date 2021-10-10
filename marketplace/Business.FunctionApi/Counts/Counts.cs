using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Business.Shared.Repositories;
using Business.Shared.Models;

namespace Business.FunctionApi.Counts
{
    public class Counts
    {
        private readonly CountsTableStore countsStore;

        public Counts(CountsTableStore countsStore)
        {
            this.countsStore = countsStore;
        }

        [FunctionName("Counts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("counts", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed counts.");

            string name = req.Query["name"];

            if (req.Headers["api-key"].ToString() == "temp-api-code")
                return new OkObjectResult(await countsStore.ReadItemsAsync());

            return new UnauthorizedResult();
        }
    }
}
