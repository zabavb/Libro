namespace BookAPI.Services.Interfaces
{
    public interface ICoverTypeService
    {
        Task<IEnumerable<string>> GetCoverTypesAsync();
        Task<string> GetCoverTypeByIdAsync(int id);
    }
}
