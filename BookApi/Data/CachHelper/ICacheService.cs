namespace BookAPI.Data.CachHelper
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
        Task UpdateListAsync<T>(string key, T item, Guid? idToRemove = null, TimeSpan expiration = default);

    }
}