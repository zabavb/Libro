using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;

namespace BookAPI.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryDto subCategoryDto);
        Task<bool> DeleteSubCategoryAsync(Guid id);
        Task<PaginatedResult<SubCategoryDto>> GetSubCategoriesAsync(int pageNumber, int pageSize, string? searchTerm, SubCategoryFilter? filter, SubCategorySort? sort);
        Task<SubCategoryDto> GetSubCategoryByIdAsync(Guid id);
        Task<SubCategoryDto> UpdateSubCategoryAsync(Guid id, SubCategoryDto subCategoryDto);
    }
}
