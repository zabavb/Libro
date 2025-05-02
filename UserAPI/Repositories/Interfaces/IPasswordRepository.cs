using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface IPasswordRepository
    {
        Task<bool> VerifyAsync(Guid userId, string plainPassword);
        Task<bool> AddAsync(Guid id, string password, Guid userId);
        Task<bool> UpdateAsync(Guid userId, string newPassword);
        Task<bool> DeleteAsync(Guid passwordId);
        Task<Password?> GetByIdAsync(Guid passwordId);
        Task<Password?> GetByUserIdAsync(Guid userId);
        Task<string> GetHashByIdAsync(Guid passwordId);
    }
}