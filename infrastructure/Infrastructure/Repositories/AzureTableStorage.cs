using Infrastructure.Abstractions;
using Infrastructure.Constants;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ITableStorage<T> where T : Microsoft.Azure.Cosmos.Table.ITableEntity
    {
        Task<IEnumerable<T>> ReadItemsAsync(string partitionKey = null, EntityResolver<T> entityResolver = null);
        Task<T> ReadItemAsync(string rowId, string partitionKey = null, EntityResolver<T> entityResolver = null);
        Task<T> CreateItemAsync(T item, string rowId = null, string partitionKey = null);
        Task<T> UpdateItemAsync(T item, string rowId = null, string partitionKey = null);
        Task<T> DeleteItemAsync(T item, string rowId = null, string partitionKey = null);
    }

    public class AzureTableStorage<T> : ITableStorage<T> where T : Abstractions.BaseEntity, new()
    {
        private const string AzureTableStorageConnectionString = "Azure:Storage:ConnectionString";
        
        private CloudStorageAccount cloudStorageAccount;
        private CloudTableClient cloudTableClient;

        protected CloudTable cloudTable;
        public ILogger<AzureTableStorage<T>> logger { get; protected set; }
        public string ResourceName { get; protected set; }
        
        public AzureTableStorage(IConfiguration configuration, ILogger<AzureTableStorage<T>> logger) 
        {
            var connectionString = configuration.GetSection(AzureTableStorageConnectionString).Value;
            cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            this.logger = logger;
        }

        private async Task<T> UpsertItem(T item, TableOperation operation, string rowId = null, string partitionKey = null)
        {
            try
            {
                item.PartitionKey ??= partitionKey ?? ResourceName;
                item.RowKey = rowId ?? item.Id;
                //item.ETag ??= ResourceName;
                item.Timestamp = DateTime.UtcNow;
                cloudTable ??= cloudTableClient.GetTableReference(ResourceName);
                var result = await cloudTable.ExecuteAsync(operation);
                T insertItem = result.Result as T;
                return insertItem;
            }
            catch (StorageException ex)
            {
                logger.LogError($"{ex.RequestInformation.ExtendedErrorInformation}");
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
            }
            finally { }
            return item;
        }

        public async Task<T> CreateItemAsync(T item, string rowId = null, string partitionKey = null) => await UpsertItem(item, TableOperation.InsertOrReplace(item), rowId, partitionKey);

        public async Task<T> UpdateItemAsync(T item, string rowId = null, string partitionKey = null) => await UpsertItem(item, TableOperation.InsertOrReplace(item), rowId, partitionKey);

        public Task<T> DeleteItemAsync(T item, string rowId = null, string partitionKey = null) => throw new NotImplementedException();

        public async Task<T> ReadItemAsync(string rowId, string partitionKey = null, EntityResolver<T> entityResolver = null)
        {
            try
            {
                cloudTable ??= cloudTableClient.GetTableReference(ResourceName);
                partitionKey ??= ResourceName;
                var retrieveOperation = entityResolver is null
                    ? TableOperation.Retrieve<T>(partitionKey, rowId, BaseModelEntityResolver)
                    : TableOperation.Retrieve<T>(partitionKey, rowId, entityResolver);
                var result = await cloudTable.ExecuteAsync(retrieveOperation);
                return result.Result as T;
            }
            catch (StorageException ex)
            {
                logger.LogError($"{ex.RequestInformation.ExtendedErrorInformation}");
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
            }
            finally
            { }
            return default(T);
        }

        public async Task<IEnumerable<T>> ReadItemsAsync(string partitionKey = null, EntityResolver<T> entityResolver = null)
        {
            var entities = new List<T>();
            try
            {
                var dynamicTableEntity = new DynamicTableEntity();
                dynamicTableEntity.Properties = TableEntity.Flatten(new T(), new OperationContext());

                cloudTable ??= cloudTableClient.GetTableReference(ResourceName);
                partitionKey ??= ResourceName;
                var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
                var query = new TableQuery<DynamicTableEntity>().Where(filter);
                var result = entityResolver is null
                    ? await cloudTable.ExecuteQuerySegmentedAsync(query, BaseModelEntityResolver, new TableContinuationToken())
                    : await cloudTable.ExecuteQuerySegmentedAsync(query, entityResolver, new TableContinuationToken());
                return result.Results;
            }
            catch (StorageException ex)
            {
                logger.LogError($"{ex.RequestInformation.ExtendedErrorInformation}");
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
            }
            finally
            { }
            return entities;
        }

        protected EntityResolver<T> BaseModelEntityResolver = (pk, rk, ts, props, etag) =>
        {
            var resolvedEntity = new T { PartitionKey = pk, RowKey = rk, Timestamp = ts, ETag = etag };
            // case for decimals which are not supported azure table types
            foreach (var prop in props.Where(p => p.Key.StartsWith(InfrastructureConstants.DecimalPrefix)))
            {
                string realPropertyName = prop.Key.Substring(InfrastructureConstants.DecimalPrefix.Length);
                System.Reflection.PropertyInfo propertyInfo = resolvedEntity.GetType().GetProperty(realPropertyName);
                propertyInfo.SetValue(resolvedEntity, Convert.ChangeType(prop.Value.StringValue, propertyInfo.PropertyType), null);
            }
            resolvedEntity.ReadEntity(props, null);
            return resolvedEntity;
        };
    }
}
