using AutoMapper;
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

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }


        public async Task<PaginatedResult<BookDto>> GetBooksAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort)
        {
            var books = await _bookRepository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            if (books == null || books.Items == null)
            {
                throw new InvalidOperationException("Failed to fetch books.");
            }

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
