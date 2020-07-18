using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrialAzureFnApp.Models;

namespace TrialAzureFnApp
{
    class AboutFaqService
    {
        [FunctionName("AboutFaq")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(Microsoft.Azure.WebJobs.Extensions.Http.AuthorizationLevel.Anonymous, "get", Route =null)] HttpRequest req,
            [CosmosDB(
                databaseName: "TrialDb", collectionName:"TrialContainer", ConnectionStringSetting ="CosmosDbConnectionString"
            )] IEnumerable<AboutFaq> itemSet,
            ILogger log)
        {
            log.LogInformation("Data fetched from TrialDb");
            return new OkObjectResult(itemSet);
        }
    }
}
