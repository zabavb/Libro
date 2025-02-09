using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderAPI.Repository;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderApi.Repository
{
    public class DeliveryTypeRepository(OrderDbContext context, IConnectionMultiplexer redis, ILogger<IDeliveryTypeRepository> logger ) : IDeliveryTypeRepository
    {
        private readonly OrderDbContext _context = context;
        private readonly IDatabase _redisDatabase = redis.GetDatabase(); 
        public readonly string _cacheKeyPrefix = "DeliveryType_";
        public readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private readonly ILogger<IDeliveryTypeRepository> _logger = logger;


        public async Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            IEnumerable<DeliveryType> deliveryTypes;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedDeliveryTypes = await _redisDatabase.HashGetAllAsync(cacheKey);
            if (cachedDeliveryTypes.Length > 0)
            {
                deliveryTypes = cachedDeliveryTypes.Select(entry => JsonSerializer.Deserialize<DeliveryType>(entry.Value!)!);
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                deliveryTypes = _context.DeliveryTypes.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = deliveryTypes.ToDictionary(
                    deliveryType => deliveryType.DeliveryId.ToString(),
                    deliveryType => JsonSerializer.Serialize(deliveryType)
                    );

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    [.. hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value))]
                    );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            var totalDeliveryTypes = await Task.FromResult(deliveryTypes.Count());

            deliveryTypes = await Task.FromResult(deliveryTypes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<DeliveryType> result = [.. deliveryTypes];
            
            return new PaginatedResult<DeliveryType>
            {
                Items = result,
                TotalCount = totalDeliveryTypes,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DeliveryType?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            string fieldKey = id.ToString();

            var cachedDeliveryType = await _redisDatabase.HashGetAsync(cacheKey,fieldKey);

            if (!cachedDeliveryType.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<DeliveryType>(cachedDeliveryType!);
            }

            _logger.LogInformation("Fetched from DB.");
            var deliveryType = await _context.DeliveryTypes.FindAsync(id);
            if(deliveryType != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    fieldKey,
                    JsonSerializer.Serialize(deliveryType)
                );

                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
            }

            return deliveryType;
        }

        public async Task CreateAsync(DeliveryType entity)
        {
                await _context.DeliveryTypes.AddAsync(entity);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryType entity)
        {
            if (!await _context.DeliveryTypes.AnyAsync(d => d.DeliveryId == entity.DeliveryId))
                throw new InvalidOperationException();

            _context.DeliveryTypes.Update(entity);
            await _context.SaveChangesAsync();
        }

        async public Task DeleteAsync(Guid id)
        {
            var deliveryType = await _context.DeliveryTypes.FindAsync(id) ?? throw new KeyNotFoundException();

            _context.DeliveryTypes.Remove(deliveryType);
            await _context.SaveChangesAsync();
        }
    }
}
