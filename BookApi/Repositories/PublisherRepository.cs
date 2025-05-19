using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Library.Common;
using BookAPI.Data.CachHelper;

namespace BookAPI.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<PublisherRepository> _logger;
        private readonly string _cacheKeyPrefix = "Publisher_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);
        private readonly ICacheService _cacheService;

        public PublisherRepository(
            BookDbContext context,
            ICacheService cacheService, ILogger<PublisherRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(Publisher entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Publishers.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration);

            string allPublishersCacheKey = $"{_cacheKeyPrefix}All";
            var cachedPublishers = await _cacheService.GetAsync<List<Publisher>>(allPublishersCacheKey) ?? new List<Publisher>();

            cachedPublishers.Add(entity);
            await _cacheService.SetAsync(allPublishersCacheKey, cachedPublishers, _cacheExpiration);

            _logger.LogInformation("New Publisher added to DB and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id == id) ?? throw new KeyNotFoundException("Publisher not found");
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allPublishersCacheKey = $"{_cacheKeyPrefix}All";
            var cachedPublishers = await _cacheService.GetAsync<List<Publisher>>(allPublishersCacheKey);

            if (cachedPublishers != null)
            {
                await _cacheService.UpdateListAsync(allPublishersCacheKey, default(Publisher), id, _cacheExpiration);
            }
        }

        public async Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm,  PublisherSort? sort)
        {

            string cacheKey = $"{_cacheKeyPrefix}All";
            List<Publisher>? publishers = await _cacheService.GetAsync<List<Publisher>>(cacheKey);
            bool isFromCache = publishers != null && publishers.Count > 0;

            if (isFromCache)
            {
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                publishers = await _context.Publishers.ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, publishers, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<Publisher> publisherQuery = publishers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                publisherQuery = isFromCache
                    ? publisherQuery.InMemorySearch(searchTerm, b => b.Name).AsQueryable()
                    : publisherQuery.SearchBy(searchTerm, b => b.Name);
            }

            if (sort != null)
                publisherQuery = sort.Apply(publisherQuery);

            var totalPublishers = publisherQuery.Count();
            var paginatedPublishers = publisherQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Publisher>
            {
                Items = paginatedPublishers,
                TotalCount = totalPublishers,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Publisher?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedPublisher = await _cacheService.GetAsync<Publisher>(cacheKey);

            if (cachedPublisher != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedPublisher;
            }

            _logger.LogInformation("Fetched from DB.");
            var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id == id);

            if (publisher != null)
            {
                await _cacheService.SetAsync(cacheKey, publisher, _cacheExpiration);
            }

            return publisher;
        }

        public async Task UpdateAsync(Publisher entity)
        {
            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id == entity.Id)
                                 ?? throw new KeyNotFoundException("Publisher not found");

            _context.Entry(existingPublisher).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration);

            string allPublishersCacheKey = $"{_cacheKeyPrefix}All";
            var cachedPublishers = await _cacheService.GetAsync<List<Publisher>>(allPublishersCacheKey);

            if (cachedPublishers != null)
            {
                await _cacheService.UpdateListAsync(allPublishersCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("Publisher updated in DB and cached.");
        }
    }
}
