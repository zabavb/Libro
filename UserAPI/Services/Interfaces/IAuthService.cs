using Google.Apis.Auth;
using UserAPI.Models.Auth;

namespace UserAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Dto?> AuthenticateAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest entity);
        Task<Dto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings);
    }
}
