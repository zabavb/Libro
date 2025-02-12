
using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IBookRepository : IManagable<Book>
    {
        Task<PaginatedResult<Book>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
        Task DeleteAsync(Guid id);
    }
}
