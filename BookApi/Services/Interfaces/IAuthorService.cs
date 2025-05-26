using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface IAuthorService
    {
        // Renamed from "GetAuthorsAsync" to "GetAllAsync"
        Task<PaginatedResult<AuthorDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            AuthorFilter? filter,
            AuthorSort? sort
        );
        Task<AuthorDto> GetByIdAsync(Guid id);
        Task CreateAsync(AuthorRequest request);
        Task UpdateAsync(Guid id, AuthorRequest request);
        Task DeleteAsync(Guid id);
    }
}