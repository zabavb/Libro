﻿using Library.Extensions;

namespace BookApi.Services
{
    public interface IAuthorService
    {
        Task<PaginatedResult<AuthorDto>> GetAuthorsAsync(int pageNumber, int pageSize);
        Task<AuthorDto> GetAuthorByIdAsync(Guid id);
        Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto);
        Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto);
        Task<bool> DeleteAuthorAsync(Guid id);
    }
}
