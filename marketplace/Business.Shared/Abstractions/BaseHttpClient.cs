using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared.Abstractions
{
    public abstract class BaseHttpClient<T> where T : BaseModel
    {
        protected HttpClient HttpClient;
        protected JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        protected ILogger<BaseHttpClient<T>> Logger;
        public string ResourceName { get; protected set; }

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

        public abstract Task<T> AddItemAsync(T item);

        public abstract Task<T> EditItemAsync(T item);

        public abstract Task DeleteItemAsync(string id, bool isHardDelete = false);

        protected StringContent GetStringContentFromObject(object o)
        {
            var serialized = JsonSerializer.Serialize(o);
            var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}
