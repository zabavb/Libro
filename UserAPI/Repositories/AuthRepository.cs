using Library.Common.Middleware;
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
            try
            {
                var user = await _context.Users
                    .AsNoTracking().FirstOrDefaultAsync(user => user.Email == request.Identifier);

                if (user == null)
                    _logger.LogError($"User not found by email.");

                return user;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to retrieve user by email.", ex);
            }
        }

        public async Task<User?> GetUserByPhoneNumberAsync(LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking().FirstOrDefaultAsync(user => user.PhoneNumber == request.Identifier);

                if (user == null)
                    _logger.LogError($"User not found by phone number.");

                return user;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"Failed to retrieve user by phone number.", ex);
            }
        }
    }
}
