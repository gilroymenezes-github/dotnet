using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared.Auth
{
    public abstract class ValuesHttpClient<T>
    {
        protected ILogger<T> logger;
        protected HttpClient httpClient;
        protected JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        protected string resourceName;

        public ValuesHttpClient(
            HttpClient httpClient,
            ILogger<T> logger
            )
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public virtual async Task<IEnumerable<T>> GetValuesAsync()
        {
            var response = await httpClient.GetAsync(httpClient.BaseAddress + $"/{resourceName}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(IList<T>)
                : JsonSerializer.Deserialize<IList<T>>(responseAsString, jsonSerializerOptions);
        }
    }
}
