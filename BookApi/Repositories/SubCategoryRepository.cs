using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;

namespace BookAPI.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly BookDbContext _context;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<SubCategoryRepository> _logger;
        private readonly string _cacheKeyPrefix = "SubCategory_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        public SubCategoryRepository(BookDbContext context, IConnectionMultiplexer redis, ILogger<SubCategoryRepository> logger)
        {
            _context = context;
            _redisDatabase = redis.GetDatabase();
            _logger = logger;
        }

        public async Task CreateAsync(SubCategory entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Subcategories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var subCategory = await _context.Subcategories.FirstOrDefaultAsync(sc => sc.Id == id)
                ?? throw new KeyNotFoundException("SubCategory not found");
            _context.Subcategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            await _redisDatabase.KeyDeleteAsync($"{_cacheKeyPrefix}{id}");
        }

        public async Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, SubCategoryFilter? filter, SubCategorySort? sort)
        {
            IQueryable<SubCategory> subcategories = _context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Books);
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                subcategories = subcategories.SearchBy(searchTerm, p => p.Name);
            }
            subcategories = filter?.Apply(subcategories) ?? subcategories;
            subcategories = sort?.Apply(subcategories) ?? subcategories;

            var totalSubcategories = await subcategories.CountAsync();
            var resultSubcategories = await subcategories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<SubCategory>
            {
                Items = resultSubcategories,
                TotalCount = totalSubcategories,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<SubCategory?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedSubCategory = await _redisDatabase.StringGetAsync(cacheKey);
            if (!cachedSubCategory.IsNullOrEmpty)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return JsonSerializer.Deserialize<SubCategory>(cachedSubCategory!);
            }

            _logger.LogInformation("Fetched from DB.");
            var subCategory = await _context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Books)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (subCategory != null)
            {
                await _redisDatabase.StringSetAsync(cacheKey, JsonSerializer.Serialize(subCategory), _cacheExpiration);
                _logger.LogInformation("Set to CACHE.");
            }
            return subCategory;
        }

        public async Task UpdateAsync(SubCategory entity)
        {
            var existingSubCategory = await _context.Subcategories.FirstOrDefaultAsync(sc => sc.Id == entity.Id)
                ?? throw new KeyNotFoundException("SubCategory not found");
            _context.Entry(existingSubCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            await _redisDatabase.StringSetAsync($"{_cacheKeyPrefix}{entity.Id}", JsonSerializer.Serialize(entity), _cacheExpiration);
        }
    }
}
