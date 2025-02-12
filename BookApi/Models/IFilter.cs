namespace BookAPI.Models
{
    public interface IFilter<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }

}
