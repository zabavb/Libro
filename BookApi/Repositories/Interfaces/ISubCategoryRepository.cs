using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IManageable<SubCategory,SubCategory>
    {
        Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, SubCategoryFilter? filter, SubCategorySort? sort);
        Task DeleteAsync(Guid id);
    }
}
