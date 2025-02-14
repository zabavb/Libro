using BookAPI.Models.Sortings;
using Library.Extensions;

namespace BookAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedResult<CategoryDto>> GetCategoriesAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort);
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CategoryDto CategoryDto);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryDto CategoryDto);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
