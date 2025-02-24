using AutoMapper;
using UserAPI.Models;
using UserAPI.Models.Auth;
using UserAPI.Repositories;

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

        public async Task<User?> Me(Guid id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while fetching user data.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task<UserDto?> AuthenticateAsync(LoginRequest request)
        {
            
            UserDto? user = request.Identifier.Contains('@') ?
                _mapper.Map<UserDto>(await _authRepository.GetUserByEmailAsync(request)) :
                _mapper.Map<UserDto>(await _authRepository.GetUserByPhoneNumberAsync(request));

            return await IsRightPassword(_mapper.Map<User>(user), request.Password) ? user : null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            User user = new User()
            {
                UserId = new Guid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var password = request.Password;    
            
            try
            {
                await _userRepository.CreateAsync(user);
                await _passwordRepository.AddAsync(password, user);
                _message = "Successful user registration in UserAPI.Services.AuthService.RegisterAsync";
                _logger.LogError(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while registering new user.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }*/
        }

        private async Task<bool> IsRightPassword(User user, string password)
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
