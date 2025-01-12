using BookApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookAPI.Infrastructure.Extensions
{
    public static class BookSearchExtensions
    {
        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return books;

            searchTerm = searchTerm.ToLower();
            return books.Where(b => b.Title.ToLower().Contains(searchTerm) ||
                                     b.Author.Name.ToLower().Contains(searchTerm) ||
                                     b.Description.ToLower().Contains(searchTerm));
        }
    }
}
