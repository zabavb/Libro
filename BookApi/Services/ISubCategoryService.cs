
namespace BookApi.Services
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryDto subCategoryDto);
        Task<bool> DeleteSubCategoryAsync(Guid id);
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync();
        Task<SubCategoryDto> GetSubCategoryByIdAsync(Guid id);
        Task<SubCategoryDto> UpdateSubCategoryAsync(Guid id, SubCategoryDto subCategoryDto);
    }
}
