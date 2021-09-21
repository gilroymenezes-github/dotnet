using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Business.Abstractions.Auth
{
    public class AuthorizationHandlerForEmail<T> : AuthorizationHandler<AuthorizationRequirementForEmail, T>  where T : BaseModel
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AuthorizationRequirementForEmail requirement,
            T resource)
        {
            if (context.User.Identity.IsAuthenticated) context.Succeed(requirement); // compare email here TBD
            return Task.CompletedTask;
        }
    }

    public class AuthorizationRequirementForEmail : IAuthorizationRequirement { }
}
