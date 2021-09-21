using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Business.Users.Abstractions.Clients;
using Business.Users.Abstractions.Models;

namespace Business.WebApp.Abstractions
{
    public class AuthenticationServiceForUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly UsersWebApiClient usersWebApiClient;
        public AuthenticationServiceForUser(
            AuthenticationStateProvider authStateProvider, 
            UsersWebApiClient usersWebApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authStateProvider = authStateProvider;
            this.usersWebApiClient = usersWebApiClient;
        }

        public async Task<string> GetIdentityUserName()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var name = authState.User.Claims.FirstOrDefault(c => c.Type == "name");
            return name.Value;
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalUser() => (await authStateProvider.GetAuthenticationStateAsync())?.User;

        public async Task<string> GetUserEmailFromIdentity()
        {
            var user = (await authStateProvider.GetAuthenticationStateAsync()).User;
            var email = user.Claims.FirstOrDefault(u => u.Type == "email")?.Value;
            return email;
        }
    }
}
