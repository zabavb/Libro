using AutoMapper;
using System.Text.RegularExpressions;
using UserAPI.Models;
using UserAPI.Models.Auth;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class AuthService(IAuthRepository authRepository, IUserRepository userRepository, IMapper mapper, ILogger<IAuthService> logger) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IAuthService> _logger = logger;
        private string _message = string.Empty;

        public async Task<UserDto?> AuthenticateAsync(LoginRequest request)
        {
            UserDto? user = request.Identifier.Contains('@') ?
                _mapper.Map<UserDto>(await _authRepository.GetUserByEmailAsync(request)) :
                _mapper.Map<UserDto>(await _authRepository.GetUserByPhoneNumberAsync(request));

            return await IsRightPassword(user, "That is password :)") ? user : null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }
            var user = _mapper.Map<User>(request);  // missing password
            try
            {
                await _userRepository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while registering new user.";
                _logger.LogError(_message); 
                throw new InvalidOperationException(_message, ex);
            }
        }

        private static async Task<bool> IsRightPassword(UserDto user, string password)
        {
            if (user != null && !string.IsNullOrWhiteSpace(password))
            {
                // Password check
            }

            return true;
        }
    }
}
