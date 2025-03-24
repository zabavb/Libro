using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;

namespace BookAPI.Services.Interfaces
{
    public interface IBookService
    {
        public Task<PaginatedResult<BookDto>> GetBooksAsync(
            int pageNumber, int pageSize, string searchTerm, BookFilter? filter, BookSort? sort);
        Task<BookDto> GetBookByIdAsync(Guid bookId);
        Task<BookDto> CreateBookAsync(BookDto bookDto, IFormFile? imageFile);
        Task<BookDto> UpdateBookAsync(Guid id, BookDto bookDto, IFormFile? imageFile);
        Task<bool> DeleteBookAsync(Guid id);


    }
}
