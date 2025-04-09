namespace Library.Interfaces
{
    public interface IManageable<T, TDetails>
    {
        Task<TDetails?> GetByIdAsync(Guid id);
        Task CreateAsync(T dto);
        Task UpdateAsync(T dto);
        Task DeleteAsync(Guid id);
    }
}