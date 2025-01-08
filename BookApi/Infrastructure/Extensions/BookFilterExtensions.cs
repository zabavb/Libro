using BookApi.Models;
using Library.Filters;

namespace BookAPI.Infrastructure.Extensions
{
    public static class BookFilterExtensions
    {
        public static IQueryable<Book> Filter(this IQueryable<Book> books, BookFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Title))
                books = books.Where(b => b.Title.ToLower().Contains(filter.Title.ToLower()));

            if (filter.AuthorId.HasValue)
                books = books.Where(b => b.AuthorId == filter.AuthorId.Value);

            if (filter.MinPrice.HasValue)
                books = books.Where(b => b.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                books = books.Where(b => b.Price <= filter.MaxPrice.Value);

            if (filter.MinYear.HasValue)
                books = books.Where(b => b.Year >= filter.MinYear.Value);

            if (filter.MaxYear.HasValue)
                books = books.Where(b => b.Year <= filter.MaxYear.Value);

            if (filter.Language.HasValue)
                books = books.Where(b => b.Language.ToString() == filter.Language.Value.ToString());

            if (filter.Cover.HasValue)
                books = books.Where(b => b.Cover.ToString() == filter.Cover.Value.ToString());

            if (filter.IsAvaliable.HasValue)
                books = books.Where(b => b.IsAvaliable == filter.IsAvaliable.Value);

            if (filter.CategoryId.HasValue)
                books = books.Where(b => b.CategoryId == filter.CategoryId.Value);

            return books;
        }
    }

}
