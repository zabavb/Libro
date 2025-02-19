using AutoMapper;
using BookApi.Controllers;
using BookApi.Models;
using BookAPI.Repositories;

namespace BookApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(ILogger<AuthorService> logger, IMapper mapper, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
            _logger = logger;
        }
        
        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();

            if (authors == null || !authors.Any())
            {
                _logger.LogWarning("No authors found");
                return Enumerable.Empty<AuthorDto>();
            }
            _logger.LogInformation("Successfully found authors");
            return _mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"No author with id {id}");
                return null;
            }

            _logger.LogInformation($"Successfully found author with id {id}");
            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            try
            {
                await _authorRepository.CreateAsync(author);
                _logger.LogInformation($"Successfully created author with id {authorDto.AuthorId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to create author. Error: {ex.Message}");
            }

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);

            if (existingAuthor == null)
            {
                _logger.LogWarning($"UpdateAuthorAsync returns null");
                return null;
            }

            try
            {
                _mapper.Map(authorDto, existingAuthor);
                await _authorRepository.UpdateAsync(existingAuthor);
                _logger.LogInformation($"Successfully updated author with id {authorDto.AuthorId}");
            }
            catch (Exception ex) 
            {
                _logger.LogWarning($"Failed to update author. Error: {ex.Message}");
            }
            return _mapper.Map<AuthorDto>(existingAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"DeleteAuthorAsync returns null");
                return false;
            }

            try
            {
                await _authorRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted author with id {id}");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogWarning($"Failed to delete autor. Error: {ex.Message}");
                return false;
            }
        }
    }
}
