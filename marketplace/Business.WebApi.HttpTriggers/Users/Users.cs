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
    public static class Users
    {
        [FunctionName("Users")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed users.");

            var users = new List<User>
                {
                    new User { Email = "user@email.in", UserId = "tbduserid" },
                };

            await Task.CompletedTask;

            if (req.Headers["api-key"].ToString() == "temp-api-code") return new OkObjectResult(users);

            return new UnauthorizedResult();
        }
    }
}
