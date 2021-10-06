using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.WebApp.Shared
{
    public class AuthenticationStateService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AuthenticationStateProvider authStateProvider;
       
        public AuthenticationStateService(
            AuthenticationStateProvider authStateProvider, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authStateProvider = authStateProvider;
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
