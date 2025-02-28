using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Extensions;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _context;

        public AuthorRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Author entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            entity.Id = Guid.NewGuid(); 
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Author not found");
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Author>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, AuthorFilter? filter, AuthorSort? sort)
        {
            IQueryable<Author> authors = _context.Authors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                authors = authors.Search(searchTerm, p => p.Name);
            }
            authors = filter?.Apply(authors) ?? authors;
            authors = sort?.Apply(authors) ?? authors;

            var totalAuthors = await authors.CountAsync();
            var resultAuthors = await authors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Author>
            {
                Items = resultAuthors,
                TotalCount = totalAuthors,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<Author?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Author entity)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Author not found");
            _context.Entry(existingAuthor).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
