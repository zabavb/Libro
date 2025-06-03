using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.Book;
using Library.DTOs.Order;
using System.Linq.Expressions;

namespace BookAPI.Services.Interfaces
{
    public interface IBookService
    {
        public Task<PaginatedResult<BookCard>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort
        );
        Task<BookDetails> GetByIdAsync(Guid bookId);
        Task<ICollection<string>?> GetAllForUserDetailsAsync(ICollection<Guid> ids);
        Task /*<BookDto>*/ CreateAsync(BookRequest bookDto);
        Task /*<BookDto>*/ UpdateAsync(Guid id, BookRequest bookDto);
        Task /*<bool>*/ DeleteAsync(Guid id);
        Task UpdateWithDiscountAsync(Guid id, UpdateBookRequest request, IDiscountService discountService);
        Task<DiscountDTO?> GetDiscountByBookIdAsync(Guid bookId);
        Task<BookOrderDetails?> GetAllForOrderDetailsAsync(Guid bookId);

    }
}
