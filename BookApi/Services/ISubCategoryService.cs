
using BookAPI.Models.Filters;
using Library.Extensions;

namespace BookApi.Services
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryDto subCategoryDto);
        Task<bool> DeleteSubCategoryAsync(Guid id);
        Task<PaginatedResult<SubCategoryDto>> GetSubCategoriesAsync(int pageNumber, int pageSize, SubCategoryFilter? filter);
        Task<SubCategoryDto> GetSubCategoryByIdAsync(Guid id);
        Task<SubCategoryDto> UpdateSubCategoryAsync(Guid id, SubCategoryDto subCategoryDto);
    }
}
