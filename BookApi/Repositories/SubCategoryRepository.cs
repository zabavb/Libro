using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Extensions;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;
using BookAPI.Data.CachHelper;
using System.Text.Json.Serialization;

namespace BookAPI.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly BookDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<SubCategoryRepository> _logger;
        private readonly string _cacheKeyPrefix = "SubCategory_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles, 
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ігнорування null значень
        };
        public SubCategoryRepository(BookDbContext context,
             ICacheService cacheService, ILogger<SubCategoryRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(SubCategory entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Subcategories.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration, _jsonOptions);

            string allSubCategory = $"{_cacheKeyPrefix}All";
            var cachedCategorys = await _cacheService.GetAsync<List<SubCategory>>(allSubCategory, _jsonOptions) ?? new List<SubCategory>();

            cachedCategorys.Add(entity);
            await _cacheService.SetAsync(allSubCategory, cachedCategorys, _cacheExpiration, _jsonOptions);

            _logger.LogInformation("New Category added to DB and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {

            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var subCategory = await _context.Subcategories.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("SubCategory not found");
            _context.Subcategories.Remove(subCategory);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allSubcategoriesCacheKey = $"{_cacheKeyPrefix}All";
            var cachedSubcategories = await _cacheService.GetAsync<List<SubCategory>>(allSubcategoriesCacheKey, _jsonOptions);

            if (cachedSubcategories != null)
            {
                await _cacheService.UpdateListAsync(allSubcategoriesCacheKey, default(SubCategory), id, _cacheExpiration);
            }
        }

        public async Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, SubCategoryFilter? filter, SubCategorySort? sort)
        {
            List<SubCategory> subcategories;
            string cacheKey = $"{_cacheKeyPrefix}All";
            var cachedSubcategories = await _cacheService.GetAsync<List<SubCategory>>(cacheKey, _jsonOptions);

            if (cachedSubcategories != null && cachedSubcategories.Count > 0)
            {
                subcategories = cachedSubcategories;
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                subcategories = await _context.Subcategories.Include(sc => sc.Category).Include(sc => sc.Books).ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, subcategories, _cacheExpiration, _jsonOptions);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<SubCategory> subcategoryQuery = subcategories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                subcategoryQuery = subcategoryQuery.Search(searchTerm, p => p.Name);

            if (filter != null)
                subcategoryQuery = filter.Apply(subcategoryQuery);

            if (sort != null)
                subcategoryQuery = sort.Apply(subcategoryQuery);

            var totalSubcategories = subcategoryQuery.Count();
            var paginatedSubcategories = subcategoryQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<SubCategory>
            {
                Items = paginatedSubcategories,
                TotalCount = totalSubcategories,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task<SubCategory?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedSubCategory = await _cacheService.GetAsync<SubCategory>(cacheKey, _jsonOptions);

            if (cachedSubCategory != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedSubCategory;
            }

            _logger.LogInformation("Fetched from DB.");
            var subCategory = await _context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Books)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (subCategory != null)
            {
                await _cacheService.SetAsync(cacheKey, subCategory, _cacheExpiration, _jsonOptions);
            }

            return subCategory;
        }

        public async Task UpdateAsync(SubCategory entity)
        {
            var existingSubCategory = await _context.Subcategories.FirstOrDefaultAsync(sc => sc.Id == entity.Id)
                                      ?? throw new KeyNotFoundException("SubCategory not found");

            _context.Entry(existingSubCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration, _jsonOptions);

            string allSubCategoriesCacheKey = $"{_cacheKeyPrefix}All";
            var cachedSubCategories = await _cacheService.GetAsync<List<SubCategory>>(allSubCategoriesCacheKey, _jsonOptions);

            if (cachedSubCategories != null)
            {
                await _cacheService.UpdateListAsync(allSubCategoriesCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("SubCategory updated in DB and cached.");
        }

    }
}
