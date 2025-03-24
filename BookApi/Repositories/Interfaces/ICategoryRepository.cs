using BookAPI.Models;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IManageable<Category>
    {
        Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort);
        Task DeleteAsync(Guid id);
    }
}
