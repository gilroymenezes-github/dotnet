using Business.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Shared.Storage
{
    public class AzureReadOnlyTableStorage<T> : IReadOnlyTableStorage<T> where T : BaseModel, new()
    {
        ILogger logger;
        
        public AzureReadOnlyTableStorage(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<T> ReadItemAsync(CloudTable cloudTable, string rowId, string partitionKey = null, EntityResolver<T> entityResolver = null)
        {
            try
            {
                partitionKey ??= cloudTable.Name;
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

        public async Task<IEnumerable<T>> ReadItemsAsync(CloudTable cloudTable, string partitionKey = null, EntityResolver<T> entityResolver = null)
        {
            var entities = new List<T>();
            try
            {
                var dynamicTableEntity = new DynamicTableEntity();
                dynamicTableEntity.Properties = TableEntity.Flatten(new T(), new OperationContext());

                partitionKey ??= cloudTable.Name;
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
            foreach (var prop in props.Where(p => p.Key.StartsWith(ApplicationConstant.DecimalPrefix)))
            {
                string realPropertyName = prop.Key.Substring(ApplicationConstant.DecimalPrefix.Length);
                System.Reflection.PropertyInfo propertyInfo = resolvedEntity.GetType().GetProperty(realPropertyName);
                propertyInfo.SetValue(resolvedEntity, Convert.ChangeType(prop.Value.StringValue, propertyInfo.PropertyType), null);
            }
            resolvedEntity.ReadEntity(props, null);
            return resolvedEntity;
        };
    }
}
