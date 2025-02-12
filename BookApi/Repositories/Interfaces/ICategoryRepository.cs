using BookAPI.Models;
using BookAPI.Models.Sortings;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface ICategoryRepository : IManagable<Category>
    {
        Task<PaginatedResult<Category>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, CategorySort? sort);
        Task DeleteAsync(Guid id);
    }
}
