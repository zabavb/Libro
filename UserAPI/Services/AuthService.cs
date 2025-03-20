using AutoMapper;
using Google.Apis.Auth;
using UserAPI.Models;
using UserAPI.Models.Auth;
using UserAPI.Repositories;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class AuthService(IAuthRepository authRepository, IUserRepository userRepository, IPasswordRepository passwordRepository, IMapper mapper, ILogger<IAuthService> logger) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IAuthService> _logger = logger;
        private string _message = string.Empty;

        public async Task<UserDto?> AuthenticateAsync(LoginRequest request)
        {
            var user = request.Identifier.Contains('@') ?
                await _authRepository.GetUserByEmailAsync(request) :
                await _authRepository.GetUserByPhoneNumberAsync(request);

            return await IsRightPasswordAsync(user!, request.Password) ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            var passwordId = Guid.NewGuid();
            var password = request.Password;

            User user = new()
            {
                UserId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = Library.DTOs.User.RoleType.USER,
                PasswordId = passwordId
            }; 
            
            try
            {
                await _passwordRepository.AddAsync(passwordId, password, user);
                await _userRepository.CreateAsync(user);
                _message = "Successful user registration in UserAPI.Services.AuthService.RegisterAsync";
                _logger.LogInformation(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while registering new user.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task<UserDto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            var user = _mapper.Map<UserDto>(await _userRepository.GetByEmailAsync(payload.Email)) ?? new UserDto
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    Role = Library.DTOs.User.RoleType.USER,
                };

            _logger.LogInformation("OAuthAsync() => return {user}", user);

            return user;
        }

        private async Task<bool> IsRightPasswordAsync(User user, string password)
        {
            try
            {
                if (user != null && !string.IsNullOrWhiteSpace(password))
                {
                    Guid passwordId = user.PasswordId;
                    return await _passwordRepository.VerifyAsync(passwordId, password);
                }
            }
            catch
            {
                _message = $"Error occurred while registering new user.";
                _logger.LogError(_message);
            }

            return false;
        }
    }
}
