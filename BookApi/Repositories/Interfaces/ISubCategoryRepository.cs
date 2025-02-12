using BookApi.Models;
using BookAPI.Models.Filters;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ISubCategoryRepository : IManagable<SubCategory>
    {
        Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize, SubCategoryFilter? filter);
        Task DeleteAsync(Guid id);
    }
}
