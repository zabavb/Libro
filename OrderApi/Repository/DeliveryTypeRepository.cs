using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderAPI.Repository;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderApi.Repository
{
    public class DeliveryTypeRepository : IDeliveryTypeRepository
    {
        private readonly OrderDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly string _cacheKeyPrefix;
        private readonly TimeSpan _cacheExpiration;
        private readonly ILogger<IDeliveryTypeRepository> _logger;
        public DeliveryTypeRepository(OrderDbContext context, IConnectionMultiplexer redis, ILogger<IDeliveryTypeRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();

            _cacheKeyPrefix = "DeliveryType_";
            _cacheExpiration = TimeSpan.FromMinutes(10);

            _logger = logger;
        }

        public async Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            IEnumerable<DeliveryType> deliveryTypes;

            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedDeliveryTypes = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedDeliveryTypes.IsNullOrEmpty)
            {
                deliveryTypes = JsonSerializer.Deserialize<ICollection<DeliveryType>>(cachedDeliveryTypes!)!;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                deliveryTypes = _context.DeliveryTypes.AsNoTracking();
                _logger.LogInformation("Fetched from DB.");

                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(deliveryTypes),
                    _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            var totalDeliveryTypes = await Task.FromResult(deliveryTypes.Count());

            deliveryTypes = await Task.FromResult(deliveryTypes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<DeliveryType> result = new List<DeliveryType>(deliveryTypes);
            
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
            var cachedDeliveryType = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedDeliveryType.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<DeliveryType>(cachedDeliveryType!);
            }

            _logger.LogInformation("Fetched from DB.");

            //var deliveryType = await _context.DeliveryTypes.FirstOrDefaultAsync(d => d.DeliveryId == id);
            var deliveryType = await _context.DeliveryTypes.FindAsync(id);
            if(deliveryType != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.StringSetAsync(
                    cacheKey,
                    JsonSerializer.Serialize(deliveryType),
                    _cacheExpiration
                    );
            }
            return deliveryType;
        }

        public async Task CreateAsync(DeliveryType deliveryType)
        {
                await _context.DeliveryTypes.AddAsync(deliveryType);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryType deliveryType)
        {
            if (!await _context.DeliveryTypes.AnyAsync(d => d.DeliveryId == deliveryType.DeliveryId))
                throw new InvalidOperationException();

            _context.DeliveryTypes.Update(deliveryType);
            await _context.SaveChangesAsync();
        }

        async public Task DeleteAsync(Guid id)
        {
            var deliveryType = await _context.DeliveryTypes.FindAsync(id);

            if (deliveryType == null)
                throw new KeyNotFoundException();

            _context.DeliveryTypes.Remove(deliveryType);
            await _context.SaveChangesAsync();
        }
    }
}
