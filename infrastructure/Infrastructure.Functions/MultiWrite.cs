using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Functions
{
    public class MultiWrite
    {
        private readonly ILogger<MultiWrite> _logger;

        public MultiWrite(ILogger<MultiWrite> log)
        {
            _logger = log;
        }

        [FunctionName("multi-write")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("names", Connection = "AzureWebJobsStorage")] CloudQueue queue,
            [Table("names", Connection = "AzureWebJobsStorage")] CloudTable table
            )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var entity = new FieldEntity { PartitionKey = "HttpTrigger", Id = Guid.NewGuid().ToString(), Field = requestBody + $" {name}" };

            var insertOperation = TableOperation.Insert(entity);

            await table.ExecuteAsync(insertOperation);

            await queue.AddMessageAsync(new CloudQueueMessage(requestBody));

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        class FieldEntity : BaseEntity
        {
            public string Field { get; set; }
        }
    }
}

