namespace UserAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> UpdateAsync(Guid userId, string newPassword);
    }
}
