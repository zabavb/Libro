
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
        Task<PaginatedResult<BookCard>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort
        );

        Task<BookDetails?> GetDetailsAsync(Guid bookId);

        Task<ICollection<string>> GetAllForUserDetailsAsync(ICollection<Guid> ids);
        Task<BookOrderDetails> GetAllForOrderDetailsAsync(Guid bookId);
    }
}
