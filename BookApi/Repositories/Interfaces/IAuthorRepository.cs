
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IAuthorRepository : IManagable<Author>
    {
        Task<PaginatedResult<Author>> GetAllAsync(int pageNumber, int pageSize, AuthorFilter? filter, AuthorSort? sort);
        Task DeleteAsync(Guid id);
    }
}
