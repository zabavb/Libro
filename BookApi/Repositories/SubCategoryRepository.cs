using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Library.Common;
using BookAPI.Data.CachHelper;
using System.Text.Json.Serialization;
using Humanizer;

namespace BookAPI.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<SubCategoryRepository> _logger;
        public SubCategoryRepository(BookDbContext context,
             ILogger<SubCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateAsync(SubCategory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Subcategories.Add(entity);
            await _context.SaveChangesAsync(); 
            if (entity.Books?.Any() == true)
            {
                var bookIds = entity.Books.Select(b => b.Id).ToList();
                var bookSubCategoryValues = string.Join(",",
                    bookIds.Select(bookId => $"('{bookId}', '{entity.Id}')")
                );

                var sqlInsert = $"INSERT INTO BookSubCategories (BookId, SubCategoryId) VALUES {bookSubCategoryValues}";

                await _context.Database.ExecuteSqlRawAsync(sqlInsert);
            }

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Guid id)
        {

            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var subCategory = await _context.Subcategories.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("SubCategory not found");
            _context.Subcategories.Remove(subCategory);
            await _context.SaveChangesAsync();

        }

        public async Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, SubCategoryFilter? filter, SubCategorySort? sort)
        {

             IQueryable<SubCategory> subcategoryQuery = _context.Subcategories
               .Select(sc => new SubCategory
               {
                   Id = sc.Id,
                   Name = sc.Name,
                   CategoryId = sc.CategoryId,
                   Category = sc.Category,
                   Books = sc.Books.Select(b => new Book { Id = b.Id }).ToList()
               })
               .AsNoTracking();



            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                subcategoryQuery = subcategoryQuery.SearchBy(searchTerm, b => b.Name);
            }

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


            _logger.LogInformation("Fetched from DB.");
            var subCategory = await _context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == id);


            return subCategory;
        }

        public async Task UpdateAsync(SubCategory entity)
        {
            var existingSubCategory = await _context.Subcategories
                .AsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Id == entity.Id)
                    ?? throw new KeyNotFoundException("SubCategory not found");

            _context.ChangeTracker.Clear();
            _context.Entry(entity).State = EntityState.Modified;

            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM BookSubCategories WHERE SubCategoryId = {0}", entity.Id);

            if (entity.Books?.Any() == true)
            {
                var bookIds = entity.Books.Select(b => b.Id).ToList();
                var bookSubCategoryValues = string.Join(",",
                    bookIds.Select(bookId => $"('{bookId}', '{entity.Id}')")
                );

                var sqlInsert = $"INSERT INTO BookSubCategories (BookId, SubCategoryId) VALUES {bookSubCategoryValues}";

                await _context.Database.ExecuteSqlRawAsync(sqlInsert);
            }

            await _context.SaveChangesAsync();
        }


    }
}
