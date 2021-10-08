using Business.Core.Customers.Models;
using Business.Shared.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.FunctionApi
{
    public static class Customers
    {
        [FunctionName("customers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("customers", Connection ="AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed customers.");

            if (req.Headers["api-key"].ToString() == "temp-api-code") 
                return new OkObjectResult(await GetCustomers(cloudTable, log));

            return new UnauthorizedResult();
        }

        private static async Task<IEnumerable<Customer>> GetCustomers(CloudTable cloudTable, ILogger log)
        {
            var customersRepository = new AzureReadOnlyTableStorage<Customer>(log);

            var customers = await customersRepository.ReadItemsAsync(cloudTable);

            return customers;
        }
    }
}

