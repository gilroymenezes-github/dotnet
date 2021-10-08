using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared.Connections
{
    public abstract class ValuesApiClientWithAuth<T>
    {
        protected HttpClient httpClient;
        protected AuthenticationStateProvider authStateProvider;
        protected AccessTokenClient authTokenService;
        protected IConfiguration configuration;
        protected JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public ValuesApiClientWithAuth(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider,
            IConfiguration configuration
            )
        {
            this.httpClient = httpClient;
            this.authStateProvider = authStateProvider;
            this.configuration = configuration;
            authTokenService = new AccessTokenClient();
        }

        protected async Task<string> RequestAuthToken()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            if (!authState.User.Identity.IsAuthenticated) return string.Empty;
            return await authTokenService.GetToken(httpClient, configuration);

        }

        protected virtual async Task<IEnumerable<T>> GetValues(string endpoint)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            httpClient.SetBearerToken(accessToken);

            var response = await httpClient.GetAsync($"{httpClient.BaseAddress}/values/{endpoint}");
            var responseString = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(responseString)
                ? default(IEnumerable<T>)
                : JsonSerializer.Deserialize<IEnumerable<T>>(responseString, jsonSerializerOptions);
        }
    }


}
