using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Shared.Abstractions
{
    public interface IRestClient<T> where T : BaseModel
    {
        Task<T> AddItemAsync(T item);
        Task DeleteItemAsync(string id, bool isHardDelete = false);
        Task<T> EditItemAsync(T item);
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetFromIdAsync(string id);
    }
}