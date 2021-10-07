using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared
{
    public abstract class BaseHttpClient<T> : IRestClient<T> where T : BaseModel
    {
        protected HttpClient HttpClient;
        protected JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        protected ILogger<BaseHttpClient<T>> Logger;
        protected string ResourceName;

        protected BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient<T>> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            var response = await HttpClient.GetAsync(HttpClient.BaseAddress + $"/{ResourceName}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(IList<T>)
                : JsonSerializer.Deserialize<IList<T>>(responseAsString, JsonSerializerOptions);
        }

        public virtual async Task<T> GetFromIdAsync(string id)
        {
            var response = await HttpClient.GetAsync(HttpClient.BaseAddress + $"/{ResourceName}/{id}");
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(T)
                : JsonSerializer.Deserialize<T>(responseAsString, JsonSerializerOptions);
        }

        public abstract Task AddItemAsync(T item);

        public abstract Task EditItemAsync(T item);

        public abstract Task DeleteItemAsync(string id, bool isHardDelete = false);

        /// <summary>
        /// Simple how-to reference
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<T> PostAsync(T item)
        {
            var response = await HttpClient.PostAsync(
               HttpClient.BaseAddress + $"/{ResourceName}",
               GetStringContentFromObject(item));
            var responseAsString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseAsString)
                ? default(T)
                : JsonSerializer.Deserialize<T>(responseAsString);
        }

        protected StringContent GetStringContentFromObject(object o)
        {
            var serialized = JsonSerializer.Serialize(o);
            var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}
