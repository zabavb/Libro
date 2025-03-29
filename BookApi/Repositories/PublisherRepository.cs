using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BookAPI.Models.Extensions;
using StackExchange.Redis;
using System.Text.Json;
using Library.Common;

namespace BookAPI.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<PublisherRepository> _logger;
        private readonly string _cacheKeyPrefix = "Publisher_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public PublisherRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<PublisherRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task CreateAsync(Publisher entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Publishers.Add(entity);
            await _context.SaveChangesAsync();
            await _redisDatabase.StringSetAsync(_cacheKeyPrefix + entity.Id, JsonSerializer.Serialize(entity));
            _logger.LogInformation("Publisher created and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var publisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Publisher not found");
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            await _redisDatabase.KeyDeleteAsync(_cacheKeyPrefix + id);
            _logger.LogInformation("Publisher deleted and cache removed.");
        }

        public async Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, PublisherSort? sort)
        {
            IQueryable<Publisher> publishers = _context.Publishers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                publishers = publishers.Search(searchTerm, p => p.Name);
            }
            publishers = sort?.Apply(publishers) ?? publishers;

            var totalPublishers = await publishers.CountAsync();
            var resultPublishers = await publishers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Publisher>
            {
                Items = resultPublishers,
                TotalCount = totalPublishers,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Publisher?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = _cacheKeyPrefix + id;
            var cachedPublisher = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedPublisher.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched Publisher from CACHE.");
                return JsonSerializer.Deserialize<Publisher>(cachedPublisher!);
            }

            var publisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id);
            if (publisher != null)
            {
                _logger.LogInformation("Fetched Publisher from DB and stored in CACHE.");
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(publisher), _cacheExpiration);
            }

            return publisher;
        }

        public async Task UpdateAsync(Publisher entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(entity));

            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Publisher not found");
            _context.Entry(existingPublisher).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            await _redisDatabase.StringSetAsync(_cacheKeyPrefix + entity.Id, JsonSerializer.Serialize(entity), _cacheExpiration);
            _logger.LogInformation("Publisher updated and cache refreshed.");
        }
    }
}
