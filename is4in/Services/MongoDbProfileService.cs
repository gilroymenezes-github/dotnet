using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4in.Services
{
    public class MongoDbProfileService : IProfileService
    {
        private readonly IMongoDbUserStore _userStore;
        
        public MongoDbProfileService(IMongoDbUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userStore.GetUserBySubjectId(subjectId);
            var claims = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue));
            if (user != null) context.AddRequestedClaims(claims);
            
            //var claims = new List<Claim>
            //{
            //    new Claim(JwtClaimTypes.Subject, user.SubjectId),
            //    new Claim(JwtClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
            //    new Claim(JwtClaimTypes.GivenName, user.Firstname),
            //    new Claim(JwtClaimTypes.FamilyName, user.Lastname),
            //    new Claim(JwtClaimTypes.Email, user.Email),
            //    new Claim(JwtClaimTypes.EmailVerified, user.IsEmailVerified.ToString().ToLower(), ClaimValueTypes.Boolean)

            //};
            //context.IssuedClaims = claims;

            await Task.FromResult(0);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userStore.GetUserBySubjectId(context.Subject.GetSubjectId());

            context.IsActive = (user != null) && user.IsActive;
            await Task.FromResult(0);
        }
    }
}
