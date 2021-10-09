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
    public class Customers
    {
        private readonly ITableStorage<Customer> customerStore;

        public Customers(ITableStorage<Customer> customerStore)
        {
            this.customerStore = customerStore;
        }

        [FunctionName("customers")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("customers", Connection ="AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed customers.");

            if (req.Headers["api-key"].ToString() == "temp-api-code") 
                return new OkObjectResult(await GetCustomers(cloudTable, log));

            return new UnauthorizedResult();
        }

        private async Task<IEnumerable<Customer>> GetCustomers(CloudTable cloudTable, ILogger log)
        {
            var customers = await customerStore.ReadItemsAsync(cloudTable);

            return customers;
        }
    }
}

