using Google.Apis.Auth;
using UserAPI.Models.Auth;

namespace UserAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest entity);
        Task<UserDto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings);
    }
}
