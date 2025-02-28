using UserAPI.Models;
using UserAPI.Models.Auth;

namespace UserAPI.Services
{
    public interface IAuthService
    {
        Task<User?> Me(Guid id);
        Task<UserDto?> AuthenticateAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest entity);
    }
}
