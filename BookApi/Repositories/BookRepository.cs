using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAsync()
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAsync(string searchQuery, string sortBy)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchQuery) ||
                    b.Description.ToLower().Contains(searchQuery) ||
                    b.Author.Name.ToLower().Contains(searchQuery));
            }

            query = sortBy switch
            {
                "popularity" => query.OrderByDescending(b => b.Feedbacks.Count),
                "newest" => query.OrderByDescending(b => b.Year),
                "cheapest" => query.OrderBy(b => b.Price),
                "expensive" => query.OrderByDescending(b => b.Price),
                _ => query.OrderBy(b => b.Title)
            };

            return await query.ToListAsync();
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateAsync(Book entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book entity)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Book not found");
            _context.Entry(existingBook).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return; 

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
