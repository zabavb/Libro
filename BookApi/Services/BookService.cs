using AutoMapper;
using BookApi.Models;
using BookAPI.Repositories;
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

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var books = await _bookRepository.GetAsync();

            if (books == null || !books.Any())
            {
                return Enumerable.Empty<BookDto>(); 
            }

            return _mapper.Map<List<BookDto>>(books); 
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync(string? searchQuery = null, string? sortBy = null)
        {
            var books = await _bookRepository.GetAsync(searchQuery, sortBy);

            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id); 

            if (book == null)
            {
                return null;
            }

            return _mapper.Map<BookDto>(book); 
        }

        public async Task<BookDto> CreateBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);  

            await _bookRepository.CreateAsync(book); 

            return _mapper.Map<BookDto>(book);  
        }

        public async Task<BookDto> UpdateBookAsync(Guid id, BookDto bookDto)
        {
            var existingBook= await _bookRepository.GetByIdAsync(id);

            if (existingBook == null)
            {
                return null;
            }

            _mapper.Map(bookDto, existingBook);
            await _bookRepository.UpdateAsync(existingBook);

            return _mapper.Map<BookDto>(existingBook);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return false; 
            }

            await _bookRepository.DeleteAsync(id); 

            return true; 
        }
    }
}
