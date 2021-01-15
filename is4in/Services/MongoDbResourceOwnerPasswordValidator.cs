using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace is4in.Services
{
    public class MongoDbResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IMongoDbUserStore _userStore;

        public MongoDbResourceOwnerPasswordValidator(IMongoDbUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isValid = await _userStore.ValidatePassword(context.UserName, context.Password);
            if (isValid)
            {
                await Task.FromResult(new GrantValidationResult(context.UserName, "password"));
            }
            await Task.FromResult(new GrantValidationResult(TokenRequestErrors.InvalidClient));
        }
    }
}
