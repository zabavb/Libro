using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{
    public  class CategoryService : ICategoryService
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid(); 

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category is not null ? _mapper.Map<CategoryDto>(category) : null;
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = categoryDto.Name;

            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(existingCategory);
        }
    }
}
