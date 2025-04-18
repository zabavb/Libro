using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface ISubCategoryService : IManageable<SubCategoryDto>
    {
        // Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryDto subCategoryDto);
        // Task<bool> DeleteSubCategoryAsync(Guid id);
        // Renamed from "GetSubCategoriesAsync" to "GetAllAsync"
        Task<PaginatedResult<SubCategoryDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            SubCategoryFilter? filter,
            SubCategorySort? sort
        );
        // Task<SubCategoryDto> GetSubCategoryByIdAsync(Guid id);
        // Task<SubCategoryDto> UpdateSubCategoryAsync(Guid id, SubCategoryDto subCategoryDto);
    }
}