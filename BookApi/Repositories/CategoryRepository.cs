using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Library.Common;
using System.Text.Json.Serialization;
using BookAPI.Data.CachHelper;
using Library.DTOs.Book;
using Category = BookAPI.Models.Category;

namespace BookAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ICategoryRepository> _logger;
        private readonly string _cacheKeyPrefix = "Category_";
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(GlobalConstants.DefaultCacheExpirationTime);

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, 
        };


        public CategoryRepository(BookDbContext context, ICacheService cacheService, ILogger<ICategoryRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task CreateAsync(Category entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            string cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
            await _cacheService.SetAsync(cacheKey, entity, _cacheExpiration, _jsonOptions);

            string allCategoriesCacheKey = $"{_cacheKeyPrefix}All";
            var cachedCategories = await _cacheService.GetAsync<List<Category>>(allCategoriesCacheKey, _jsonOptions) ?? new List<Category>();

            cachedCategories.Add(entity);
            await _cacheService.SetAsync(allCategoriesCacheKey, cachedCategories, _cacheExpiration, _jsonOptions);

            _logger.LogInformation("New Category added to DB and cached.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new KeyNotFoundException("Category not found");
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKeyPrefix}{id}");

            string allCategoriesCacheKey = $"{_cacheKeyPrefix}All";
            var cachedCategories = await _cacheService.GetAsync<List<Category>>(allCategoriesCacheKey, _jsonOptions);

            if (cachedCategories != null)
            {
                await _cacheService.UpdateListAsync(allCategoriesCacheKey, default(Category), id, _cacheExpiration);
            }
        }

        public async Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort)
        {
            string cacheKey = $"{_cacheKeyPrefix}All";
            List<Category>? categories = await _cacheService.GetAsync<List<Category>>(cacheKey, _jsonOptions);
            bool isFromCache = categories != null && categories.Count > 0;


            if (isFromCache)
            {
                _logger.LogInformation("Fetched from CACHE.");
            }
            else
            {
                categories = await _context.Categories.ToListAsync();
                _logger.LogInformation("Fetched from DB.");

                await _cacheService.SetAsync(cacheKey, categories, _cacheExpiration, _jsonOptions);
                _logger.LogInformation("Set to CACHE.");
            }

            IQueryable<Category> categoriesQuery = categories.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                categoriesQuery = isFromCache
                    ? categoriesQuery.InMemorySearch(searchTerm, b => b.Name).AsQueryable()
                    : categoriesQuery.SearchBy(searchTerm, b => b.Name);
            }

            if (sort != null)
                categoriesQuery = sort.Apply(categoriesQuery);

            var totalCategories = categories.Count;
            categories = categoriesQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedResult<Category>
            {
                Items = categories,
                TotalCount = totalCategories,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            string cacheKey = $"{_cacheKeyPrefix}{id}";
            var cachedCategory = await _cacheService.GetAsync<Category>(cacheKey, _jsonOptions);

            if (cachedCategory != null)
            {
                _logger.LogInformation("Fetched from CACHE.");
                return cachedCategory;
            }

            _logger.LogInformation("Fetched from DB.");
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                await _cacheService.SetAsync(cacheKey, category, _cacheExpiration, _jsonOptions);
            }

            return category;
        }

        public async Task UpdateAsync(Category entity)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == entity.Id)
                                    ?? throw new KeyNotFoundException("Category not found");
            
            _context.Entry(existingCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();

            await _cacheService.SetAsync($"{_cacheKeyPrefix}{entity.Id}", entity, _cacheExpiration, _jsonOptions);

            string allCategoriesCacheKey = $"{_cacheKeyPrefix}All";
            var cachedCategories = await _cacheService.GetAsync<List<Category>>(allCategoriesCacheKey, _jsonOptions);

            if (cachedCategories != null)
            {
                await _cacheService.UpdateListAsync(allCategoriesCacheKey, entity, null, _cacheExpiration);
            }

            _logger.LogInformation("Category updated in DB and cached.");
        }

    }
}