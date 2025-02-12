using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IManagable<SubCategory>
    {
        Task<PaginatedResult<SubCategory>> GetAllAsync(
            int pageNumber, int pageSize, 
            SubCategoryFilter? filter, SubCategorySort? sort);
        Task DeleteAsync(Guid id);
    }
}
