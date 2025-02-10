namespace Library.Interfaces
{
    public interface IManagable<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
