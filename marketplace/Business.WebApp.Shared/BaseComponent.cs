using Business.Shared;
using Business.Shared.Auth.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Radzen;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.WebApp.Shared
{
    public abstract class BaseComponent : ComponentBase, IAsyncDisposable
    {
        [Inject] protected AuthenticationServiceForUser AuthenticatedUserService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected IAuthorizationService AuthorizedPolicyService { get; set; }

        protected CommandEnum Mode = CommandEnum.None;
        protected HubConnection HubConnection;

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is null) return;
            await HubConnection.DisposeAsync();
        }

        virtual protected async Task StartHubConnection(string broadcaster)
        {
            var hubConnectionString = Configuration.GetSection("PowerUnitApi").Value;
            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{hubConnectionString}/notifications")
                .Build();
            HubConnection.On<string, string>($"{broadcaster}", (user, message) =>
            {
                StateHasChanged();
                var encodedMsg = $"{user}: {message}";
                NotificationService.Notify(NotificationSeverity.Info, encodedMsg);
            });
            await HubConnection.StartAsync();
        }

        virtual protected void Clear()
        {
            Mode = CommandEnum.None;
        }

        virtual protected async Task<bool> IsAuthorizedForPolicy(BaseModel resourceItem, string policyName)
        {
            var user = await AuthenticatedUserService.GetClaimsPrincipalUser();

            var authorizationResult = await AuthorizedPolicyService.AuthorizeAsync(user, resourceItem, policyName);

            return authorizationResult.Succeeded ? true : false;
        }

        virtual protected async Task<bool> HasWritePermission(BaseModel resourceItem)
        {
            var user = await AuthenticatedUserService.GetClaimsPrincipalUser();

            var authorizationResult = await AuthorizedPolicyService.AuthorizeAsync(user, resourceItem, ReadWritePermission.Write);

            return authorizationResult.Succeeded ? true : false;
        }

        protected async Task<string> GetUserRole()
        {
            var principalUser = await AuthenticatedUserService.GetClaimsPrincipalUser();

            var roles = ((ClaimsIdentity)principalUser.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            return roles?.FirstOrDefault();
        }
    }
}
