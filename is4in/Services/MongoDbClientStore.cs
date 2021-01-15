using IdentityServer4.Models;
using IdentityServer4.Stores;
using is4in.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace is4in.Services
{
    public class MongoDbClientStore : IClientStore
    {
        private const string ClientsCollectionName = "Clients";
        private readonly IMongoDatabase _db;
        
        public MongoDbClientStore(MongoDbRepository repository)
        {
            _db = repository.Database;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await GetMongoDbClientByClientId(clientId);
            return client is null
                ? new Client()
                : new Client()
                {
                    ClientId = client.ClientId,
                    AllowedScopes = client.AllowedScopes,
                    RedirectUris = client.RedirectUris,
                    ClientSecrets = client.ClientSecrets
                };
        }

        private async Task<MongoDbClient> GetMongoDbClientByClientId(string clientId)
        {
            var collection = _db.GetCollection<MongoDbClient>(ClientsCollectionName);
            var filter = Builders<MongoDbClient>.Filter.Eq(x => x.ClientId, clientId);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }
    }
}
