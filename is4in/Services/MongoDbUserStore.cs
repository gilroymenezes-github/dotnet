using is4in.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace is4in.Services
{
    public class MongoDbUserStore : IMongoDbUserStore
    {
        private const string UsersCollectionName = "Users";
        private readonly IPasswordHasher<MongoDbUser> _passwordHasher;
        private readonly IMongoDatabase _db;
        
        public MongoDbUserStore(MongoDbRepository repository, IPasswordHasher<MongoDbUser> passHash)
        {
            _passwordHasher = passHash;
            _db = repository.Database;
        }
        
        public Task<MongoDbUser> AutoProvisionUser(string provider, string subjectId, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<MongoDbUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await GetUserBySubjectId(userId);
        }

        public async Task<MongoDbUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await GetUserByUsername(normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<MongoDbUser> GetUserByExternalProvider(string provider, string subjectId)
        {
            throw new NotImplementedException();
        }

        public async Task<MongoDbUser> GetUserBySubjectId(string subjectId)
        {
            var collection = _db.GetCollection<MongoDbUser>(UsersCollectionName);
            var filter = Builders<MongoDbUser>.Filter.Eq(u => u.SubjectId, subjectId);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<MongoDbUser> GetUserByUsername(string username)
        {
            var collection = _db.GetCollection<MongoDbUser>(UsersCollectionName);
            var filter = Builders<MongoDbUser>.Filter.Eq(u => u.UserName, username);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public Task<string> GetUserIdAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(MongoDbUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(MongoDbUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(MongoDbUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidatePassword(string username, string password)
        {
            var user = await GetUserByUsername(username);
            if (user is null) return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result switch
            {
                PasswordVerificationResult.Success => true,
                PasswordVerificationResult.Failed => default(bool),
                PasswordVerificationResult.SuccessRehashNeeded => default(bool),
                _ => default(bool)
            };

        }
    }
}
