using BookApi.Data;
using BookApi.Models;
using BookAPI.Infrastructure.Extensions;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Library.Filters;
using Library.Sortings;
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

      
        public async Task<PaginatedResult<Book>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort)
        {
            IQueryable<Book> books = _context.Books.AsQueryable();

            if (books.Any() && !string.IsNullOrWhiteSpace(searchTerm))
                books = books.Search(searchTerm);
            if (books.Any() && filter != null)
                books = books.Filter(filter);
            if (books.Any() && sort != null)
                books = books.Sort(sort);

            var totalBooks = await books.CountAsync();
            var resultBooks = await books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Book>
            {
                Items = resultBooks,
                TotalCount = totalBooks,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

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
