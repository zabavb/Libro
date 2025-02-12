using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly BookDbContext _context;

        public SubCategoryRepository(BookDbContext context)
        {
            _context = context;
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
        }

        public async Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, SubCategoryFilter? filter, SubCategorySort? sort)
        {
            IQueryable<SubCategory> subcategories = _context.Subcategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Books);

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
            return await _context.Subcategories
                .Include(sc => sc.Category) 
                .Include(sc => sc.Books)   
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task UpdateAsync(SubCategory entity)
        {
            var existingSubCategory = await _context.Subcategories.FirstOrDefaultAsync(sc => sc.Id == entity.Id)
                ?? throw new KeyNotFoundException("SubCategory not found");

            _context.Entry(existingSubCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
