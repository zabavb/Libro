using BookApi.Data;
using BookApi.Models;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _context;
        public CategoryRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Category entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Category not found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize)
        {
            IQueryable<Category> categories = _context.Categories.AsQueryable();

            var totalCategories = await categories.CountAsync();
            var resultCategories = await categories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Category>
            {
                Items = resultCategories,
                TotalCount = totalCategories,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Category entity)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Category  not found");
            _context.Entry(existingCategory).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
