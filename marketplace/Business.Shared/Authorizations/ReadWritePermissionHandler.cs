using Business.Shared.Abstractions;
using Business.Shared.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace Business.Shared.Authorizations
{
    public class ReadWritePermissionHandler<T> : AuthorizationHandler<OperationAuthorizationRequirement, T> where T : BaseModel
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
            if (requirement.Name == ReadWritePermission.Write.Name && context.User.IsInRole(ApplicationConstant.SysAdminRoleName))
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
