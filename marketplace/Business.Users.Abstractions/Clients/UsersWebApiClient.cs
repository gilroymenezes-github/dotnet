using Business.Abstractions;
using Business.Abstractions.Auth;
using Business.Users.Abstractions.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Business.Users.Abstractions.Clients
{
    public class UsersWebApiClient : BaseApiClientWithAuth<User>
    {
        public UsersWebApiClient(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider, 
            IConfiguration configuration, 
            ILogger<BaseClient<User>> logger) 
            : base(httpClient, authStateProvider, configuration, logger)
        {
            ResourceName = "users";
        }
    }
}
