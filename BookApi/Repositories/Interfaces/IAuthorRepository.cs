
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IAuthorRepository : IManageable<Author,Author>
    {
        Task<PaginatedResult<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, AuthorFilter? filter, AuthorSort? sort);
        Task DeleteAsync(Guid id);
    }
}
