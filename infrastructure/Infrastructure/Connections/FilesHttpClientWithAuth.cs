using IdentityModel.Client;
using Infrastructure.Abstractions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Connections
{
    public class FilesHttpClientWithAuth<T> : BaseHttpClientWithAuth<T> where T : FileEntity
    {
       
        public FilesHttpClientWithAuth(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<BaseHttpClient<T>> logger)
            : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "files";
        }

        public async Task<T> UploadPdfFile(T fileModel, MultipartFormDataContent fileContent)
        {
            var accessToken = await RequestAuthToken();
            if (string.IsNullOrEmpty(accessToken)) return default;
            HttpClient.SetBearerToken(accessToken);

            var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}/{ResourceName}/upload/pdf", fileContent);
            var responseString = await response.Content.ReadAsStringAsync();
            fileModel.Url = responseString;
            return fileModel;
        }
    }
}
