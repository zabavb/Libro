using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface ICategoryService : IManageable<CategoryDto>
    {
        // Renamed from "GetCategoriesAsync" to "GetAllAsync"
        Task<PaginatedResult<CategoryDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            CategorySort? sort
        );

        /*Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CategoryDto CategoryDto);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, CategoryDto CategoryDto);
        Task<bool> DeleteCategoryAsync(Guid id);*/
    }
}