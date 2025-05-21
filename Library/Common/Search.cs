using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Library.Common
{
    public static class Search
    {
        /*        public static IQueryable<T> SearchBy<T>(this IQueryable<T> query, string searchTerm,
                    params Expression<Func<T, string?>>[] searchFields)
                {
                    if (string.IsNullOrWhiteSpace(searchTerm) || searchFields.Length == 0)
                        return query;

                    searchTerm = $"%{searchTerm.ToLower()}%";

                    var parameter = Expression.Parameter(typeof(T), "x");

                    Expression? predicate = null;

                    foreach (var field in searchFields)
                    {
                        *//*var property = Expression.Invoke(field, parameter);
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var containsMethod =
                            typeof(DbFunctionsExtensions).GetMethod("Like",
                                [typeof(DbFunctions), typeof(string), typeof(string)]);

                        if (toLowerMethod != null && containsMethod != null)
                        {
                            var toLowerExpression = Expression.Call(property, toLowerMethod);
                            var likeExpression = Expression.Call(null, containsMethod, Expression.Constant(EF.Functions),
                                toLowerExpression, Expression.Constant(searchTerm));

                            predicate = predicate == null ? likeExpression : Expression.OrElse(predicate, likeExpression);
                        }*//*
                        // Access the property directly (MemberExpression)
                        var member = ReplaceParameter(field.Body, field.Parameters[0], parameter);

                        var toLower = Expression.Call(member,
                            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);

                        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                            nameof(DbFunctionsExtensions.Like),
                            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
                        )!;

                        var likeCall = Expression.Call(
                            null,
                            likeMethod,
                            Expression.Constant(EF.Functions),
                            toLower,
                            Expression.Constant(searchTerm)
                        );

                        predicate = predicate == null ? likeCall : Expression.OrElse(predicate, likeCall);
                    }

                    *//*if (predicate == null)
                        return query;*/
        /*var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
        return query.Where(lambda);*//*

        return query.Where(Expression.Lambda<Func<T, bool>>(predicate!, parameter));
    }*/


        public static IEnumerable<T> InMemorySearch<T>(this IEnumerable<T> source, string searchTerm,
            params Func<T, string?>[] fields)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || fields.Length == 0)
                return source;

            searchTerm = searchTerm.ToLower();

            return source.Where(item =>
                fields.Any(field =>
                {
                    var value = field(item);
                    return value != null && value.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase);
                }));
        }

        // Replaces all instances of `source` in `expression` with `target`
        private static Expression ReplaceParameter(Expression expression, ParameterExpression source, Expression target)
        {
            return new ReplaceVisitor(source, target).Visit(expression)!;
        }

        private class ReplaceVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _source;
            private readonly Expression _target;

            public ReplaceVisitor(ParameterExpression source, Expression target)
            {
                _source = source;
                _target = target;
            }

            protected override Expression VisitParameter(ParameterExpression node) =>
                node == _source ? _target : base.VisitParameter(node);
        }
    }



}