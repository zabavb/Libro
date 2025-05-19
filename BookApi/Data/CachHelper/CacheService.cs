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

        public async Task<T?> GetAsync<T>(string key, JsonSerializerOptions? jsonOptions = null)
        {
            var cachedValue = await _redisDatabase.StringGetAsync(key);
            if (cachedValue.IsNullOrEmpty)
            {
                _logger.LogInformation($"Cache miss for key {key}");
                return default;
            }

            _logger.LogInformation($"Cache hit for key {key}");
            return JsonSerializer.Deserialize<T>(cachedValue, jsonOptions ?? new JsonSerializerOptions());
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration, JsonSerializerOptions? jsonOptions = null)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value, jsonOptions ?? new JsonSerializerOptions());
                await _redisDatabase.StringSetAsync(key, serializedValue, expiration);
                _logger.LogInformation($"Set value in cache for key {key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to set cache for key {key}. Trying to remove the key.");
                try
                {
                    await RemoveAsync(key);
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, $"Failed to delete key {key} after set failure.");
                }
            }
        }


        public async Task RemoveAsync(string key)
        {
            await _redisDatabase.KeyDeleteAsync(key);
            _logger.LogInformation($"Removed key {key} from cache");
        }

        public async Task UpdateListAsync<T>(string key, T item, Guid? idToRemove = null, TimeSpan expiration = default, JsonSerializerOptions? jsonOptions = null)
        {
            var list = await GetAsync<List<T>>(key, jsonOptions) ?? new List<T>();

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

            await SetAsync(key, list, expiration, jsonOptions);
        }
    }
}
