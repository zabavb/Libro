using AutoMapper;
using BookApi.Models;
using BookAPI.Repositories;

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

        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();

            if (authors == null || !authors.Any())
            {
                return Enumerable.Empty<AuthorDto>();
            }

            return _mapper.Map<List<AuthorDto>>(authors);
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
