﻿using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Library.Sortings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookAPI.Models.Extensions;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

      
        public async Task<PaginatedResult<Book>> GetAllAsync(
            int pageNumber, 
            int pageSize, 
            string searchTerm, 
            BookFilter? filter,
            BookSort? sort)
        {

            IQueryable<Book> books = _context.Books
                .Include(x => x.Subcategories)
                .Include(x=>x.Feedbacks).AsQueryable();

            if (books.Any() && !string.IsNullOrWhiteSpace(searchTerm))
                books = books.Search(searchTerm, b => b.Title, b => b.Author.Name);
            books = filter?.Apply(books) ?? books;
            books = sort?.Apply(books) ?? books;


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
            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Subcategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            return book;
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
