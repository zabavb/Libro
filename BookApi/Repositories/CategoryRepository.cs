using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Extensions;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;
using System.Text.Json.Serialization;

namespace BookAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<ICategoryRepository> _logger;
        private readonly string _cacheKeyPrefix = "Category_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public CategoryRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<ICategoryRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task CreateAsync(Category entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id)
                              ?? throw new KeyNotFoundException("Category not found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            await _redisDatabase.KeyDeleteAsync($"{_cacheKeyPrefix}{id}");
        }

        public async Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort)
        {
            IQueryable<Category> categories;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedCategories = await _redisDatabase.HashGetAllAsync(cacheKey);

            if (cachedCategories.Length > 0)
            {
                categories = cachedCategories.Select(entry => JsonSerializer.Deserialize<Category>(entry.Value!, _jsonSerializerOptions)!)
                                             .AsQueryable();
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                categories = _context.Categories.Include(b => b.Subcategories).AsQueryable();
                _logger.LogInformation("Fetched from DB.");

                var hashEntries = categories.ToDictionary(
                    category => category.Id.ToString(),
                    category => JsonSerializer.Serialize(category, _jsonSerializerOptions)
                );

                await _redisDatabase.HashSetAsync(
                    cacheKey,
                    hashEntries.Select(kvp => new HashEntry(kvp.Key, kvp.Value)).ToArray()
                );
                await _redisDatabase.KeyExpireAsync(cacheKey, _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
                categories = categories.Search(searchTerm, b => b.Name);
            categories = sort?.Apply(categories) ?? categories;

            var totalCategories = categories.Count();
            categories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PaginatedResult<Category>
            {
                Items = categories.ToList(),
                TotalCount = totalCategories,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedCategory = await _redisDatabase.StringGetAsync(cacheKey);

            if (!cachedCategory.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<Category>(cachedCategory!, _jsonSerializerOptions);
            }

            _logger.LogInformation("Fetched from DB.");
            var category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);

            if (category != null)
            {
                _logger.LogInformation("Set to CACHE.");
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(category, _jsonSerializerOptions), _cacheExpiration);
            }

            return category;
        }

        public async Task UpdateAsync(Category entity)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(a => a.Id == entity.Id)
                                    ?? throw new KeyNotFoundException("Category not found");
            _context.Entry(existingCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _redisDatabase.StringSetAsync($"{_cacheKeyPrefix}{entity.Id}", JsonSerializer.Serialize(entity, _jsonSerializerOptions), _cacheExpiration);
        }
    }
}
