using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Shared.Abstractions
{
    public interface IRepository<T, T2> 
    {
        Task<IEnumerable<T>> ReadItemsAsync(string queryString);
        Task<T> ReadItemAsync(string id);
        Task<T2> CreateItemAsync(string id, T item);
        Task<T2> UpdateItemAsync(string id, T item);
        Task<T2> DeleteItemAsync(string id);
    }
}
