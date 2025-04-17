namespace BookAPI.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<string> GetByIdAsync(int id);
    }
}
