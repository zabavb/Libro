using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;

namespace BookAPI.Services.Interfaces
{
    public interface IBookService
    {
        // Renamed from "GetBooksAsync" to "GetAllAsync"
        public Task<PaginatedResult<BookDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort
        );

        Task<BookDto> GetByIdAsync(Guid bookId);
        Task<ICollection<string>?> GetAllForUserDetailsAsync(ICollection<Guid> ids);
        Task /*<BookDto>*/ CreateAsync(BookDto bookDto, IFormFile? imageFile);
        Task /*<BookDto>*/ UpdateAsync(Guid id, BookDto bookDto, IFormFile? imageFile);
        Task /*<bool>*/ DeleteAsync(Guid id);
    }
}