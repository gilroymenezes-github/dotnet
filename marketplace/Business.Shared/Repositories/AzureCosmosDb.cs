using Business.Shared;
using Business.Shared.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Shared.Repositories
{
    public class AzureCosmosDb<T, ItemResponse> : IRepository<T, ItemResponse<T>> where T : BaseModel 
    {
        private const string AzureCosmosDbName = "Azure:CosmosDbSql:DatabaseName";
        private const string AzureCosmosDbSqlConnectionString = "Azure:CosmosDbSql:ConnectionString";
        private string connectionString;
        private string databaseName;
        private CosmosClientOptions cosmosClientOptions;
        private Container container;

        protected string resourceName;
        protected Container GetContainer()
            => container
            ??= new CosmosClient(connectionString, cosmosClientOptions)
            .GetContainer(databaseName, $"{resourceName}");

        public AzureCosmosDb(IConfiguration configuration)
        {
            cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions 
                { 
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase 
                }
            };
            connectionString = configuration.GetSection(AzureCosmosDbSqlConnectionString).Value;
            databaseName = configuration.GetSection(AzureCosmosDbName).Value;
        }
                
        public async Task<ItemResponse<T>> CreateItemAsync(string id, T item) => await GetContainer().CreateItemAsync(item, new PartitionKey(id));

        public async Task<ItemResponse<T>> DeleteItemAsync(string id) => await GetContainer().DeleteItemAsync<T>(id, new PartitionKey(id));

        public async Task<ItemResponse<T>> UpdateItemAsync(string id, T item) => await GetContainer().UpsertItemAsync(item, new PartitionKey(id));

        public async Task<T> ReadItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await GetContainer().ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(T);
            }
        }

        public async Task<IEnumerable<T>> ReadItemsAsync(string queryString)
        {
            var query = GetContainer().GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<ItemResponse<T>> ReplaceItemAsync(string id, T item) => await GetContainer().ReplaceItemAsync(item, id);
    }
}
