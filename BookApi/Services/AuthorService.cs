using AutoMapper;
using BookApi.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Extensions;

namespace BookApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IMapper mapper, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public async Task<PaginatedResult<AuthorDto>> GetAuthorsAsync(int pageNumber, int pageSize, AuthorFilter? filter, AuthorSort? sort)
        {
            var authors = await _authorRepository.GetAllAsync(pageNumber, pageSize, filter, sort);
            if (authors == null || authors.Items == null)
            {
                throw new InvalidOperationException("Failed to fetch authors.");
            }

            return new PaginatedResult<AuthorDto>
            {
                Items = _mapper.Map<ICollection<AuthorDto>>(authors.Items),
                TotalCount = authors.TotalCount,
                PageNumber = authors.PageNumber,
                PageSize = pageSize
            };
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            await _authorRepository.CreateAsync(author);

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);

            if (existingAuthor == null)
            {
                return null;
            }

            _mapper.Map(authorDto, existingAuthor); 
            await _authorRepository.UpdateAsync(existingAuthor);

            return _mapper.Map<AuthorDto>(existingAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return false;
            }

            await _authorRepository.DeleteAsync(id);
            return true;
        }
    }
}
