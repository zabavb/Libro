using Library.Extensions;

namespace BookApi.Services
{
    public interface IBookService
    {
        public Task<PaginatedResult<BookDto>> GetBooksAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
        Task<BookDto> GetBookByIdAsync(Guid bookId);
        Task<BookDto> CreateBookAsync(BookDto bookDto);
        Task<BookDto> UpdateBookAsync(Guid id, BookDto bookDto);
        Task<bool> DeleteBookAsync(Guid id);


    }
}
