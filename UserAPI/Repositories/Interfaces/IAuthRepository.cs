using UserAPI.Models;
using UserAPI.Models.Auth;

namespace UserAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(LoginRequest request);
        Task<User?> GetUserByPhoneNumberAsync(LoginRequest request);
    }
}
