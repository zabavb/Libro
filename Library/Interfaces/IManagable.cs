namespace Library.Interfaces
{
    public interface IManageable<T, TDetails>
    {
        Task<TDetails?> GetByIdAsync(Guid id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
