using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IManagable<Category>
    {
        Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteAsync(Guid id);
    }
}
