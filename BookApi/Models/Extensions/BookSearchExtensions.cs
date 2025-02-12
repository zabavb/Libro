//using BookAPI.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace BookAPI.Infrastructure.Extensions
//{
//    public static class BookSearchExtensions
//    {
//        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
//        {
//            if (string.IsNullOrEmpty(searchTerm))
//                return books;

//            searchTerm = searchTerm.ToLower();
//            return books.Where(b => b.Title.ToLower().Contains(searchTerm) ||
//                                     b.Author.Name.ToLower().Contains(searchTerm) ||
//                                     b.Description.ToLower().Contains(searchTerm));
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookAPI.Models.Extensions
{
    public static class SearchExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> query, string searchTerm, params Expression<Func<T, string>>[] searchFields)
        {

            if (string.IsNullOrWhiteSpace(searchTerm) || searchFields.Length == 0)
                return query;

            searchTerm = $"%{searchTerm.ToLower()}%";

            var parameter = Expression.Parameter(typeof(T), "x");

            Expression? predicate = null;

            foreach (var field in searchFields)
            {
                var property = Expression.Invoke(field, parameter);
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(DbFunctionsExtensions).GetMethod("Like", new[] { typeof(DbFunctions), typeof(string), typeof(string) });

                if (toLowerMethod != null && containsMethod != null)
                {
                    var toLowerExpression = Expression.Call(property, toLowerMethod);
                    var likeExpression = Expression.Call(null, containsMethod, Expression.Constant(EF.Functions), toLowerExpression, Expression.Constant(searchTerm));

                    predicate = predicate == null ? likeExpression : Expression.OrElse(predicate, likeExpression);
                }
            }

            if (predicate == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
            return query.Where(lambda);
        }
    }
}

