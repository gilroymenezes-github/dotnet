using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Shared
{
    public interface IRestClient<T> where T : BaseModel
    {
        Task AddItemAsync(T item);
        Task DeleteItemAsync(string id, bool isHardDelete = false);
        Task EditItemAsync(T item);
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetFromIdAsync(string id);
    }
}