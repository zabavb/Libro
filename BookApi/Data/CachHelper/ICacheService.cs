using System.Text.Json;

namespace BookAPI.Data.CachHelper
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, JsonSerializerOptions? jsonOptions = null);
        Task SetAsync<T>(string key, T value, TimeSpan expiration, JsonSerializerOptions? jsonOptions = null);
        Task RemoveAsync(string key);
        Task UpdateListAsync<T>(string key, T item, Guid? idToRemove = null, TimeSpan expiration = default, JsonSerializerOptions? jsonOptions = null);
    }
}
