using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.DTOs.Book;
using System.Linq.Expressions;

namespace BookAPI.Services.Interfaces
{
    public interface IBookService
    {
        public Task<PaginatedResult<BookDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort
        );
        Task<BookDto> GetByIdAsync(Guid bookId);
        Task<ICollection<string>?> GetAllForUserDetailsAsync(ICollection<Guid> ids);
        Task /*<BookDto>*/ CreateAsync(BookRequest bookDto);
        Task /*<BookDto>*/ UpdateAsync(Guid id, BookRequest bookDto);
        Task /*<bool>*/ DeleteAsync(Guid id);
        Task<int> GetQuantityById(Guid id);
        Task AddQuantityById(Guid id, int quantity);
        // temporary?
        Task<List<BookDto>> GetBooksByConditionAsync(Expression<Func<Models.Book, bool>> condition);
        Task UpdateWithDiscountAsync(Guid id, UpdateBookRequest request, IDiscountService discountService);
        Task<DiscountDTO?> GetDiscountByBookIdAsync(Guid bookId);

    }
}
