using BookApi.Models;
using Library.Sortings;

namespace BookAPI.Infrastructure.Extensions
{
    public static class BookSortExtensions
    {
        public static IQueryable<Book> Sort(this IQueryable<Book> books, BookSort sort)
        {
            if (sort.Title != Bool.NULL)
                books = sort.Title == Bool.ASCENDING
                    ? books.OrderBy(b => b.Title)
                    : books.OrderByDescending(b => b.Title);

            if (sort.Price != Bool.NULL)
                books = sort.Price == Bool.ASCENDING
                    ? books.OrderBy(b => b.Price)
                    : books.OrderByDescending(b => b.Price);

            if (sort.Year != Bool.NULL)
                books = sort.Year == Bool.ASCENDING
                    ? books.OrderBy(b => b.Year)
                    : books.OrderByDescending(b => b.Year);

            if (sort.Language != Bool.NULL)
                books = sort.Language == Bool.ASCENDING
                    ? books.OrderBy(b => b.Language)
                    : books.OrderByDescending(b => b.Language);

            if (sort.Cover != Bool.NULL)
                books = sort.Cover == Bool.ASCENDING
                    ? books.OrderBy(b => b.Cover)
                    : books.OrderByDescending(b => b.Cover);

            if (sort.IsAvaliable != Bool.NULL)
                books = sort.IsAvaliable == Bool.ASCENDING
                    ? books.OrderBy(b => b.IsAvaliable)
                    : books.OrderByDescending(b => b.IsAvaliable);

            return books;
        }
    }

}
