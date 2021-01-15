using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace is4in.Services
{
    public class MongoDbRepository
    {
        public IMongoDatabase Database { get; private set; }

        public MongoDbRepository(IOptions<MongoDbRepositoryConfiguration> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            Database = client.GetDatabase(config.Value.DatabaseName);
        }
    }
}
