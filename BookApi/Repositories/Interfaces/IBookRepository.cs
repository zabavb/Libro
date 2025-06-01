
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.Order;
using Library.Interfaces;
using System.Linq.Expressions;

namespace BookAPI.Repositories.Interfaces
{
    public interface IBookRepository : IManageable<Book>
    {
        Task<PaginatedResult<Book>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort
        );

        Task<ICollection<string>> GetAllForUserDetailsAsync(ICollection<Guid> ids);
        Task<BookOrderDetails> GetAllForOrderDetailsAsync(Guid bookId);
    }
}
