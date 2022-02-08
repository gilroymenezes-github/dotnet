using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Authorizations
{
    public class ReadWritePermissionHandler<T> : AuthorizationHandler<OperationAuthorizationRequirement, T> where T : BaseEntity
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            T resource)
        {
            if (requirement.Name == ReadWritePermission.Read.Name && context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }
            if (requirement.Name == ReadWritePermission.Write.Name)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public static class ReadWritePermission
    {
        public static OperationAuthorizationRequirement Write
            = new OperationAuthorizationRequirement { Name = nameof(Write) };

        public static OperationAuthorizationRequirement Read
            = new OperationAuthorizationRequirement { Name = nameof(Read) };
    }
}
