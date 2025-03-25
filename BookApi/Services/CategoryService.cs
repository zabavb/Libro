using AutoMapper;
using BookAPI.Models;
using BookAPI.Models.Sortings;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;

namespace BookAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid();

            try
            {
                await _categoryRepository.CreateAsync(category);
                _logger.LogInformation($"Successfully created category with id {category.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to create category. Error: {ex.Message}");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                _logger.LogWarning($"DeleteCategoryAsync returns null");

                return false; 
            }
            try
            {
                await _categoryRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted category with id {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to delete category. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<PaginatedResult<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort)
        {
            var categories = await _categoryRepository.GetAllAsync(pageNumber, pageSize, searchTerm, sort);

            if (categories == null || categories.Items == null)
            {
                _logger.LogWarning("No categories found");
                throw new InvalidOperationException("Failed to fetch categories.");
            }
            _logger.LogInformation("Successfully found categories");
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

            if (category == null)
            {
                _logger.LogWarning($"No category with id {id}");
                return null;
            }
            _logger.LogInformation($"Successfully found category with id {id}");
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
            {
                _logger.LogWarning($"UpdateCategoryAsync returns null");
                return null;
            }

            try
            {
                _mapper.Map(categoryDto, existingCategory);
                await _categoryRepository.UpdateAsync(existingCategory);
                _logger.LogInformation($"Successfully updated category with id {id}");

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to update category. Error: {ex.Message}");
            }
            return _mapper.Map<CategoryDto>(existingCategory);
        }
    }
}
