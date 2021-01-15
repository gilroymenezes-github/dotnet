using is4in.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace is4in.Services
{
    public interface IMongoDbUserStore : IUserStore<MongoDbUser>
    {
        Task<bool> ValidatePassword(string username, string password);
        Task<MongoDbUser> GetUserBySubjectId(string subjectId);
        Task<MongoDbUser> GetUserByUsername(string username);
        Task<MongoDbUser> GetUserByExternalProvider(string provider, string subjectId);
        Task<MongoDbUser> AutoProvisionUser(string provider, string subjectId, List<Claim> claims);
        //Task<bool> AddOrUpdateUser(MongoDbUser user, string newPasswordToHash = null);
    }
}