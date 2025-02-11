using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface ISubCategoryRepository: IManagable<SubCategory>
    {
        Task<PaginatedResult<SubCategory>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteAsync(Guid id);
    }
}
