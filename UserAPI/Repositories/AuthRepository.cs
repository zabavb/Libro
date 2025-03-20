using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Models.Auth;

namespace UserAPI.Repositories
{
    public class AuthRepository(UserDbContext context, ILogger<IAuthRepository> logger) : IAuthRepository
    {
        private readonly UserDbContext _context = context;
        private readonly ILogger<IAuthRepository> _logger = logger;

        public async Task<User?> GetUserByEmailAsync(LoginRequest request)
        {
            var user = await _context.Users
                .AsNoTracking().FirstOrDefaultAsync(user => user.Email == request.Identifier);

            if (user == null)
                _logger.LogError($"User with email [{request.Identifier}] not found.");

            return user;
        }

        public async Task<User?> GetUserByPhoneNumberAsync(LoginRequest request)
        {
            var user = await _context.Users
                .AsNoTracking().FirstOrDefaultAsync(user => user.PhoneNumber == request.Identifier);

            if (user == null)
                _logger.LogError($"User with phone number [{request.Identifier}] not found.");

            return user;
        }
    }
}
