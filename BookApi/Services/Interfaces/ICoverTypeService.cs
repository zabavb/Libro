namespace BookAPI.Services.Interfaces
{
    public interface ICoverTypeService
    {
        // Renamed from "GetCoberTypesAsync" to "GetAllAsync"
        Task<IEnumerable<string>> GetAllAsync();
        Task<string> GetByIdAsync(int id);
    }
}