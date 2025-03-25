using System.Linq.Expressions;

namespace Library.Sortings
{
    public abstract class SortBase<T>
    {
        public abstract IQueryable<T> Apply(IQueryable<T> query);

        protected virtual IQueryable<T> ApplySorting<TProp>(
            IQueryable<T> query, Bool sortOrder, Expression<Func<T, TProp>> keySelector)
        {
            return sortOrder switch
            {
                Bool.ASCENDING => query.OrderBy(keySelector),
                Bool.DESCENDING => query.OrderByDescending(keySelector),
                _ => query
            };
        }
    }
}
