using AutoMapper;
using BookApi.Controllers;
using BookApi.Models;
using BookAPI.Repositories;
using Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<PaginatedResult<BookDto>> GetBooksAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort)
        {
            var books = await _bookRepository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            if (books == null || books.Items == null)
            {
                _logger.LogWarning("No books found");
                throw new InvalidOperationException("Failed to fetch books.");
            }
            _logger.LogInformation("Successfully found books");
            return new PaginatedResult<BookDto>
            {
                Items = _mapper.Map<ICollection<BookDto>>(books.Items),
                TotalCount = books.TotalCount,
                PageNumber = books.PageNumber,
                PageSize = books.PageSize
            };
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id); 

            if (book == null)
            {
                _logger.LogWarning($"No book with id {id}");
                return null;
            }

            _logger.LogInformation($"Successfully found book with id {id}");
            return _mapper.Map<BookDto>(book); 
        }

        public async Task<BookDto> CreateBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            try
            {
                await _bookRepository.CreateAsync(book);
                _logger.LogInformation($"Successfully created book with id {bookDto.BookId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to create book. Error: {ex.Message}");
            }

            return _mapper.Map<BookDto>(book);  
        }

        public async Task<BookDto> UpdateBookAsync(Guid id, BookDto bookDto)
        {
            var existingBook= await _bookRepository.GetByIdAsync(id);

            if (existingBook == null)
            {
                _logger.LogWarning($"UpdateBookAsync returns null");
                return null;
            }

            try
            {
                _mapper.Map(bookDto, existingBook);
                await _bookRepository.UpdateAsync(existingBook);
                _logger.LogInformation($"Successfully updated book with id {bookDto.BookId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to update book. Error: {ex.Message}");
            }
            return _mapper.Map<BookDto>(existingBook);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning($"DeleteBookAsync returns null");
                return false; 
            }

            try
            {
                await _bookRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to delete book. Error: {ex.Message}");
                return false;
            }
        }
    }
}
