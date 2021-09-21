using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Business.WebAsm
{
    /// <summary>
    /// Role based access is deprected for Business.Admin. See Business.Rules.AccessPolicy instead.
    /// </summary>
    /// <typeparam name="TAccount"></typeparam>
    public class MultipleRoleClaimsPrincipalFactory<TAccount> : AccountClaimsPrincipalFactory<TAccount>
        where TAccount : RemoteUserAccount
    {
        public MultipleRoleClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor)
            : base(accessor)
        {

        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(TAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            var claimsIdentity = (ClaimsIdentity)user.Identity;

            if (account != null) MapArrayClaimsToMultipleSeparateClaims(account, claimsIdentity);

            return user;
        }

        private static void MapArrayClaimsToMultipleSeparateClaims(TAccount account, ClaimsIdentity claimsIdentity)
        {
            foreach(var prop in account.AdditionalProperties)
            {
                var key = prop.Key;
                var value = prop.Value;
                if (value != null &&
                    (value is JsonElement element && element.ValueKind == JsonValueKind.Array))
                {
                    claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(prop.Key));
                    var claims = element.EnumerateArray().Select(x => new Claim(prop.Key, x.ToString()));
                    claimsIdentity.AddClaims(claims);
                }
            }
        }
    }
}
