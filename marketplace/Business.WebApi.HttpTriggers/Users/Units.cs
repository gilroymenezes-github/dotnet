using Business.Users.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.WebApi.HttpTriggers.Users
{
    public static class Units
    {
        [FunctionName("Units")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed units.");

            var units = new List<Unit>
                {
                    new Unit { Name = "Creometric" },
                    new Unit { Name = "Fathamster" },
                    new Unit { Name = "Kodework" },
                    new Unit { Name = "Ninestack" }
                };

            await Task.CompletedTask;

            if (req.Headers["api-key"].ToString() == "temp-api-code") return new OkObjectResult(units);

            return new UnauthorizedResult();
        }
    }
}
