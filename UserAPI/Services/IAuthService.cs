using UserAPI.Models.Auth;

namespace UserAPI.Services
{
    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest entity);
    }
}
