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
        private readonly ILogger<ICategoryRepository> _logger;

        


        public CategoryRepository(BookDbContext context, ILogger<ICategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateAsync(Category entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            if (entity.Subcategories != null)
            {
                var newSubcategories = await _context.Subcategories
                    .Where(sc => entity.Subcategories.Select(s => s.Id).Contains(sc.Id))
                    .ToListAsync();

                entity.Subcategories.Clear();
                foreach (var sub in newSubcategories)
                {
                    entity.Subcategories.Add(sub);
                }
            }
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New Category added to DB.");
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id) ?? throw new KeyNotFoundException("Category not found");
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort)
        {
            var categories = await _context.Categories.Include(c => c.Subcategories).ToListAsync();
            _logger.LogInformation("Fetched from DB.");
            IQueryable<Category> categoriesQuery = categories.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                categoriesQuery = categoriesQuery.SearchBy(searchTerm, b => b.Name);
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

            _logger.LogInformation("Fetched from DB.");
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async Task UpdateAsync(Category entity)
        {
            var existingCategory = await _context.Categories
                .Include(c => c.Subcategories).
                FirstOrDefaultAsync(c => c.Id == entity.Id)
                                    ?? throw new KeyNotFoundException("Category not found");

            existingCategory.Name = entity.Name;

            if (entity.Subcategories != null)
            {
                var newSubcategories = await _context.Subcategories
                    .Where(sc => entity.Subcategories.Select(s => s.Id).Contains(sc.Id))
                    .ToListAsync();

                existingCategory.Subcategories.Clear();
                foreach (var sub in newSubcategories)
                {
                    existingCategory.Subcategories.Add(sub);
                }
            }
            await _context.SaveChangesAsync();

            _logger.LogInformation("Category updated in DB and cached.");
        }

    }
}