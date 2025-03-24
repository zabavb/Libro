using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;

namespace BookAPI.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<PaginatedResult<AuthorDto>> GetAuthorsAsync(int pageNumber, int pageSize, string? searchTerm, AuthorFilter? filter, AuthorSort? sort);
        Task<AuthorDto> GetAuthorByIdAsync(Guid id);
        Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto);
        Task<bool> DeleteAuthorAsync(Guid id);
    }
}
