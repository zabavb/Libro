using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UserAPI.Models.Searches
{
    public static class UserSearch
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
                var containsMethod = typeof(DbFunctionsExtensions).GetMethod("Like", [typeof(DbFunctions), typeof(string), typeof(string)]);

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
