namespace BookAPI.Models.Filters
{
    public interface IFilter<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }

}
