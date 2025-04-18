using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface IAuthorService : IManageable<AuthorDto>
    {
        // Renamed from "GetAuthorsAsync" to "GetAllAsync"
        Task<PaginatedResult<AuthorDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            AuthorFilter? filter,
            AuthorSort? sort
        );
        /*Task<AuthorDto> GetAuthorByIdAsync(Guid id);
        Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto);
        Task<bool> DeleteAuthorAsync(Guid id);*/
    }
}