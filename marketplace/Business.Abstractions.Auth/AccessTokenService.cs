using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Abstractions.Auth
{
    public class AccessTokenService
    {

        public async Task<string> GetToken(
            HttpClient httpClient, 
            IConfiguration configuration)
        {
            try
            {
                var discovery = await HttpClientDiscoveryExtensions.GetDiscoveryDocumentAsync(httpClient, configuration.GetSection("Oidc:Authority").Value);

                if (discovery.IsError)
                {
                    throw new ApplicationException($"Error: {discovery.Error}");
                }

                var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(
                    httpClient,
                    new ClientCredentialsTokenRequest
                    {
                        Scope = "businessapi",
                        ClientSecret = "businessapisecret",
                        Address = discovery.TokenEndpoint,
                        ClientId = "business.api"
                    });

                if (tokenResponse.IsError)
                {
                    throw new ApplicationException($"Error: {tokenResponse.Error}");
                }

                return tokenResponse.AccessToken;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {e}");
            }
            finally
            {
            }
            return string.Empty;
        }
    }
}
