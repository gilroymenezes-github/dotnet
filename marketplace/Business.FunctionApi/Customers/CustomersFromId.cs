using Business.Core.Customers.Models;
using Business.Shared.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;

namespace Business.FunctionApi
{
    public class CustomersFromId
    {
        private readonly ITableStorage<Customer> customerStore;

        public CustomersFromId(ITableStorage<Customer> customerStore)
        {
            this.customerStore = customerStore;
        }

        [FunctionName("customers-fromid")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "customers/{id}")] HttpRequest req,
            [Table("customers", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed customers-fromid.");

            var id = req.Path.ToUriComponent().Split("/")?.Last();

            if (string.IsNullOrEmpty(id)) 
                return new BadRequestResult();

            if (req.Headers["api-key"].ToString() == "temp-api-code") 
                return new OkObjectResult(await GetCustomerFromId(cloudTable, id, log));

            return new UnauthorizedResult();
        }

        private async Task<Customer> GetCustomerFromId(CloudTable cloudTable, string id, ILogger log)
        {
            var customer = await customerStore.ReadItemAsync(cloudTable, id);

            return customer;
        }
    }
}

