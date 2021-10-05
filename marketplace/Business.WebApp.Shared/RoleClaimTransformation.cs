using Business.Shared;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.WebApp.Shared
{
    public class RoleClaimTransformation : IClaimsTransformation
    {
       
        
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Cloning required on original identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            var username = principal.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (username is null) return principal;
            var emailPrefix = username.Split("@")?[0];
            if (emailPrefix is null) return principal;

            var roles = new List<string> { ApplicationConstant.SysAdminRoleName, ApplicationConstant.SelfRegUserRoleName };
            var role = roles.FirstOrDefault(r => r.ToLower() == emailPrefix);
            if (role is null) return principal;

            var claim = new Claim(newIdentity.RoleClaimType, role);
            newIdentity.AddClaim(claim);

            return clone;
        }
    }
}
