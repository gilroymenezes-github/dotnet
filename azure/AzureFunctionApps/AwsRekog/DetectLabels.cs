using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwsRekog
{
    public class DetectLabels
    {
        private readonly ILogger<DetectLabels> _logger;

        public DetectLabels(ILogger<DetectLabels> log)
        {
            _logger = log;
        }

        [FunctionName("detect-labels")]
        [OpenApiOperation(operationId: "DetectLabelsResult")]
        //[OpenApiRequestBody(contentType: "image/jpeg", bodyType: typeof(IFormFile), Description = "The **Data** body", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<DetectLabelsResult>), Description = "The OK response")]
        public async Task<IActionResult> DetectLabelsResult(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "detect-labels")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var image = new Amazon.Rekognition.Model.Image();

            using var ms = new MemoryStream();
            var file = req.Form.Files[0];
            await file.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            image.Bytes = ms;

            var rekognitionClient = new AmazonRekognitionClient();
            var detectLabelsRequest = new DetectLabelsRequest
            {
                Image = image,
                MaxLabels = 5,
                MinConfidence = 75F
            };

            DetectLabelsResponse detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectLabelsRequest);

            var detectLabelsResult = detectLabelsResponse?.Labels?.Select(d => new DetectLabelsResult
            {
                Name = d.Name,
                Confidence = d.Confidence
            });

            var responseMessage = System.Text.Json.JsonSerializer.Serialize<IEnumerable<DetectLabelsResult>>(detectLabelsResult);

            return new OkObjectResult(responseMessage);
        }
    }
}

