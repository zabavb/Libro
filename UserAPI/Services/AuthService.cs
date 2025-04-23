using AutoMapper;
using Google.Apis.Auth;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;
using UserAPI.Models.Auth;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class AuthService(
        IAuthRepository authRepository,
        IUserService userService,
        IUserRepository userRepository,
        IPasswordRepository passwordRepository,
        IMapper mapper,
        ILogger<IAuthService> logger) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IUserService _userService = userService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IAuthService> _logger = logger;

        public async Task<Dto?> AuthenticateAsync(LoginRequest request)
        {
            var user = request.Identifier.Contains('@')
                ? await _authRepository.GetUserByEmailAsync(request)
                : await _authRepository.GetUserByPhoneNumberAsync(request);

            Guid passwordId;
            var password = await _passwordRepository.GetByUserIdAsync(user!.UserId);
            if (password != null)
                passwordId = password.PasswordId;
            else
            {
                passwordId = Guid.NewGuid();
                await _passwordRepository.AddAsync(passwordId, request.Password, user.UserId);
            }

            return await IsRightPasswordAsync(passwordId, request.Password) ? _mapper.Map<Dto>(user) : null;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request == null)
                throw new ArgumentException("User data is required.", nameof(request));

            User user = new()
            {
                UserId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Role = RoleType.USER
            };

            await _userService.CreateAsync(_mapper.Map<Dto>(user));
            await _passwordRepository.AddAsync(Guid.NewGuid(), request.Password, user.UserId);
            _logger.LogInformation("Successful user registration.");
        }

        public async Task<Dto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            var user = _mapper.Map<Dto>(await _userRepository.GetByEmailAsync(payload.Email)) ?? new Dto
            {
                Id = Guid.Empty,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                Role = RoleType.USER,
            };

            _logger.LogInformation("OAuthAsync() => return {user}", user);

            return user;
        }

        private async Task<bool> IsRightPasswordAsync(Guid passwordId, string password)
        {
            if (!string.IsNullOrWhiteSpace(password))
                return await _passwordRepository.VerifyAsync(passwordId, password);

            return false;
        }
    }
}