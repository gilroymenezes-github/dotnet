using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BingSearch
{
    public class SearchImages
    {
        private readonly ILogger<SearchImages> _logger;
        
        private const string QUERY_PARAMETER = "?q=";  // Required
        private const string MKT_PARAMETER = "&mkt=";  // Strongly suggested
        private const string COUNT_PARAMETER = "&count=";
        private const string OFFSET_PARAMETER = "&offset=";
        private const string ID_PARAMETER = "&id=";
        private const string SAFE_SEARCH_PARAMETER = "&safeSearch=";
        private const string ASPECT_PARAMETER = "&aspect=";
        private const string COLOR_PARAMETER = "&color=";
        private const string FRESHNESS_PARAMETER = "&freshness=";
        private const string HEIGHT_PARAMETER = "&height=";
        private const string WIDTH_PARAMETER = "&width=";
        private const string IMAGE_CONTENT_PARAMETER = "&imageContent=";
        private const string IMAGE_TYPE_PARAMETER = "&imageType=";
        private const string LICENSE_PARAMETER = "&license=";
        private const string MAX_FILE_SIZE_PARAMETER = "&maxFileSize=";
        private const string MIN_FILE_SIZE_PARAMETER = "&minFileSize=";
        private const string MAX_HEIGHT_PARAMETER = "&maxHeight=";
        private const string MIN_HEIGHT_PARAMETER = "&minHeight=";
        private const string MAX_WIDTH_PARAMETER = "&maxWidth=";
        private const string MIN_WIDTH_PARAMETER = "&minWidth=";
        private const string SIZE_PARAMETER = "&size=";

        private string _subscriptionKey = "eb2b2e0c5b364777a0c9f0f84edf9da7";
        private string _baseUri = "https://api.bing.microsoft.com/v7.0/images/search";
        private string _insightsToken;
        private string _clientIdHeader;
        private long _nextOffset = 0;

        public SearchImages(ILogger<SearchImages> log)
        {
            _logger = log;
        }

        [FunctionName("search-images")]
        [OpenApiOperation(operationId: "GetSearchImagesResult", tags: new[] { "phrase" })]
        [OpenApiParameter(name: "phrase", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **search phrase** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchImagesResult>), Description = "The OK response")]
        public async Task<IActionResult> GetSearchImagesResult(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string phrase = req.Query["phrase"];

            var responseResults = await RunSearchImagesAsync(phrase);

            var responseMessage = System.Text.Json.JsonSerializer.Serialize<IEnumerable<SearchImagesResult>>(responseResults);

            return new OkObjectResult(responseMessage);
        }

        async Task<IEnumerable<SearchImagesResult>> RunSearchImagesAsync(string searchString)
        {
            var sb = new StringBuilder(QUERY_PARAMETER + Uri.EscapeDataString(searchString));
            sb.Append(SAFE_SEARCH_PARAMETER + SafeSearchEnum.Strict.ToString());
            sb.Append(MKT_PARAMETER + "en-us");
            var queryString = sb.ToString();

            HttpResponseMessage response = await MakeRequestAsync(queryString);

            var json = await response.Content.ReadAsStringAsync();
            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
            var jsonPropertyValues = jsonElement.GetProperty("value");
            var responseValues = new List<SearchImagesResult>();
            for(var i = 0; i < 4; i++)
            {
                responseValues.Add(new SearchImagesResult
                {
                    Name = jsonPropertyValues[i].GetProperty("name").GetString(),
                    Url = jsonPropertyValues[i].GetProperty("contentUrl").GetString(),
                });
            }
            return responseValues;
        }
             
        async Task<HttpResponseMessage> MakeRequestAsync(string queryString)
        {
            var client = new HttpClient();
                        
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            return (await client.GetAsync(_baseUri + queryString));
        }
    }
}

