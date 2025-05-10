namespace Library.Interfaces
{
    public interface IManageable<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task CreateAsync(T dto);
        Task UpdateAsync(T dto);
        Task DeleteAsync(Guid id);
    }
}