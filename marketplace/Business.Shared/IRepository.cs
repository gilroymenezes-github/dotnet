using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Shared
{
    public interface IRepository<T, T2> 
    {
        Task<IEnumerable<T>> ReadItemsAsync(string queryString);
        Task<T> ReadItemAsync(string id);
        Task<T2> CreateItemAsync(string id, T item);
        Task<T2> UpdateItemAsync(string id, T item);
        Task<T2> DeleteItemAsync(string id);
    }

    public interface ITableRepository<T> where T: ITableEntity
    {
        Task<T> CreateItemAsync(T item, string rowId = null, string partitionKey = null);
        Task<T> UpdateItemAsync(T item, string rowId = null, string partitionKey = null);
        Task<T> DeleteItemAsync(T item, string rowId = null, string partitionKey = null);
    }

    public interface IReadTableRepository<T> where T : ITableEntity
    {
        Task<IEnumerable<T>> ReadItemsAsync(CloudTable cloudTable, string paritionKey = null, EntityResolver<T> entityResolver = null);
        Task<T> ReadItemAsync(CloudTable cloudTable, string rowId, string partitionKey= null, EntityResolver<T> entityResolver = null);

    }
}
