using AutoMapper;
using BookApi.Models;
using BookApi.Repositories;
using BookAPI.Repositories;
using Library.Extensions;

namespace BookApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid();

            await _categoryRepository.CreateAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }

        public async Task<PaginatedResult<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize)
        {
            var categories = await _categoryRepository.GetAllAsync(pageNumber, pageSize);

            if (categories == null || categories.Items == null)
            {
                throw new InvalidOperationException("Failed to fetch categories.");
            }

            return new PaginatedResult<CategoryDto>
            {
                Items = _mapper.Map<ICollection<CategoryDto>>(categories.Items),
                TotalCount = categories.TotalCount,
                PageNumber = categories.PageNumber,
                PageSize = categories.PageSize
            };
        }


        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category is not null ? _mapper.Map<CategoryDto>(category) : null;
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
            {
                return null;
            }

            _mapper.Map(categoryDto, existingCategory);
            await _categoryRepository.UpdateAsync(existingCategory);

            return _mapper.Map<CategoryDto>(existingCategory);
        }
    }
}
