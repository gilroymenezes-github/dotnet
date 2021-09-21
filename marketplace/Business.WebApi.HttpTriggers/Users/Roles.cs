using Business.Users.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Business.WebApi.HttpTriggers.Users
{
    public static class Roles
    {
        [FunctionName("Roles")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed projecttypes.");

            var roles = new List<Role>
                {
                    new Role { Name = "Viewer" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Administrator" }
                };

            if (req.Headers["api-key"].ToString() == "temp-api-code") return new OkObjectResult(roles);

            return new UnauthorizedResult();
        }
    }
}
