using Business.Shared.Connections;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared.Abstractions
{
    public abstract class BaseHttpClientWithAuth<T> : BaseHttpClient<T> where T: BaseModel
    {
        protected AccessTokenClient authTokenService;
        protected AuthenticationStateProvider authStateProvider;
        protected IConfiguration configuration;
        
        public BaseHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<BaseHttpClient<T>> logger)
            : base(httpClient, logger)
        {
            this.authStateProvider = authStateProvider;
            this.configuration = configuration;
            authTokenService = new AccessTokenClient();
        }

        public async Task<string> RequestAuthToken()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            if (!authState.User.Identity.IsAuthenticated) return string.Empty;
            return await authTokenService.GetToken(HttpClient, configuration);
            
        }

        public override async Task<T> AddItemAsync(T item)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return item;
            HttpClient.SetBearerToken(accessToken);

            var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}/{ResourceName}", GetStringContentFromObject(item));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseString)
                ? item
                : JsonSerializer.Deserialize<T>(responseString, JsonSerializerOptions);
        }
        public override async Task<T> EditItemAsync(T item)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return item;
            HttpClient.SetBearerToken(accessToken);

            var response = await HttpClient.PutAsync($"{HttpClient.BaseAddress}/{ResourceName}/{item.Id}", GetStringContentFromObject(item));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseString)
                ? item
                : JsonSerializer.Deserialize<T>(responseString, JsonSerializerOptions);
        }

        public override async Task<IEnumerable<T>> GetAsync()
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            HttpClient.SetBearerToken(accessToken);

            var response = await HttpClient.GetAsync($"{HttpClient.BaseAddress}/{ResourceName}");
            var responseString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseString)
                ? default(IEnumerable<T>)
                : JsonSerializer.Deserialize<List<T>>(responseString, JsonSerializerOptions);
        }

        public override async Task<T> GetFromIdAsync(string id)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            HttpClient.SetBearerToken(accessToken);

            var responseString = await HttpClient.GetStringAsync($"{HttpClient.BaseAddress}/{ResourceName}/{id}");
            return string.IsNullOrEmpty(responseString)
                ? default(T)
                : JsonSerializer.Deserialize<T>(responseString, JsonSerializerOptions);
        }

        public override async Task DeleteItemAsync(string id, bool isHardDelete = false)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return;
            HttpClient.SetBearerToken(accessToken);

            var response = await HttpClient.DeleteAsync($"{HttpClient.BaseAddress}/{ResourceName}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
