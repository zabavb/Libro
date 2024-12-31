using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using Library.DTOs.Book;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly BookDbContext _context;


        public AuthorService(IMapper mapper, BookDbContext context)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync()
        {
            var authors = await _context.Authors.ToListAsync();

            if (authors == null || !authors.Any())
            {
                return Enumerable.Empty<AuthorDto>();
            }

            return _mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return null;
            }

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> CreateAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            author.Id = Guid.NewGuid(); 

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(Guid id, AuthorDto authorDto)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (existingAuthor == null)
            {
                return null;
            }

            existingAuthor.Name = authorDto.Name;
            existingAuthor.Biography = authorDto.Biography;
            existingAuthor.DateOfBirth = authorDto.DateOfBirth;

            await _context.SaveChangesAsync();

            return _mapper.Map<AuthorDto>(existingAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return false;
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
