using Business.Shared.Abstractions;
using Business.Shared.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.Shared.Connections
{
    public class FilesHttpClientWithAuth<T> : BaseHttpClientWithAuth<T> where T : FileModel
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

            var response = await HttpClient.PostAsync($"{HttpClient.BaseAddress}/{ResourceName}/upload-pdf/{fileModel.Name}", fileContent);
            var responseString = await response.Content.ReadAsStringAsync();
            fileModel.Url = responseString;
            return fileModel;
        }
    }
}
