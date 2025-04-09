using AutoMapper;
using Google.Apis.Auth;
using Library.DTOs.User;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;
using UserAPI.Models.Auth;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
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

        public async Task<Dto?> AuthenticateAsync(LoginRequest request)
        {
            var user = request.Identifier.Contains('@') ?
                await _authRepository.GetUserByEmailAsync(request) :
                await _authRepository.GetUserByPhoneNumberAsync(request);

            return await IsRightPasswordAsync(user!, request.Password) ? _mapper.Map<Dto>(user) : null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
                throw new ArgumentException("User data is required.", nameof(request));

            var passwordId = Guid.NewGuid();
            var password = request.Password;

            User user = new()
            {
                UserId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = RoleType.USER,
                PasswordId = passwordId
            }; 
            
            await _passwordRepository.AddAsync(passwordId, password, user);
            await _userRepository.CreateAsync(user);
            _logger.LogInformation("Successful user registration.");
        }

        public async Task<Dto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            var user = _mapper.Map<Dto>(await _userRepository.GetByEmailAsync(payload.Email)) ?? new Dto
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    Role = RoleType.USER,
                };

            _logger.LogInformation("OAuthAsync() => return {user}", user);

            return user;
        }

        private async Task<bool> IsRightPasswordAsync(User user, string password)
        {
            if (user != null && !string.IsNullOrWhiteSpace(password))
            {
                Guid passwordId = user.PasswordId;
                return await _passwordRepository.VerifyAsync(passwordId, password);
            }

            return false;
        }
    }
}
