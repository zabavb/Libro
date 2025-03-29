using StackExchange.Redis;
using System.Text.Json;

namespace BookAPI.Data.CachHelper
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
        {
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedValue = await _redisDatabase.StringGetAsync(key);
            if (cachedValue.IsNullOrEmpty)
            {
                _logger.LogInformation($"Cache miss for key {key}");
                return default;
            }

            _logger.LogInformation($"Cache hit for key {key}");
            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _redisDatabase.StringSetAsync(key, serializedValue, expiration);
            _logger.LogInformation($"Set value in cache for key {key}");
        }

        public async Task RemoveAsync(string key)
        {
            await _redisDatabase.KeyDeleteAsync(key);
            _logger.LogInformation($"Removed key {key} from cache");
        }

        public async Task UpdateListAsync<T>(string key, T item, Guid? idToRemove = null, TimeSpan expiration = default)
        {
            var list = await GetAsync<List<T>>(key) ?? new List<T>();

            if (idToRemove != null)
            {
                var itemToRemove = list.FirstOrDefault(i => (i as dynamic).Id == idToRemove);
                if (itemToRemove != null)
                {
                    list.Remove(itemToRemove);
                    _logger.LogInformation($"Removed item with Id {idToRemove} from list in cache");
                }
            }

            if (item != null)
            {
                var existingItem = list.FirstOrDefault(i => (i as dynamic).Id == (item as dynamic).Id);
                if (existingItem != null)
                {
                    list.Remove(existingItem); 
                    _logger.LogInformation($"Updated item with Id {(item as dynamic).Id} in list");
                }

                list.Add(item);
                _logger.LogInformation($"Added/Updated item with Id {(item as dynamic).Id} to list in cache");
            }

            await SetAsync(key, list, expiration);
        }

    }
}
