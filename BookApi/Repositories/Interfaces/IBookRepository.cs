
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;
using System.Linq.Expressions;

namespace BookAPI.Repositories.Interfaces
{
    public interface IBookRepository : IManageable<Book,Book>
    {
        Task<PaginatedResult<Book>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, BookFilter? filter, BookSort? sort);
        Task<List<Book>> GetBooksByConditionAsync(Expression<Func<Book, bool>> condition);
        Task<int> GetQuantityById(Guid id);
        Task AddQuantityById(Guid id, int quantity);

    }
}
