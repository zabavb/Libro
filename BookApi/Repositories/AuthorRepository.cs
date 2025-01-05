using BookApi.Data;
using BookApi.Models;
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
            entity.Id = Guid.NewGuid(); 
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                throw new KeyNotFoundException("Author not found");
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Author entity)
        {
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException("Author not found");
            }

            existingAuthor.Name = entity.Name;
            existingAuthor.Biography = entity.Biography;
            existingAuthor.DateOfBirth = entity.DateOfBirth;

            await _context.SaveChangesAsync();
        }
    }
}
