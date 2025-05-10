using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<IPasswordRepository> _logger;

        public PasswordRepository(UserDbContext context, ILogger<IPasswordRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        public async Task<Password?> GetByIdAsync(Guid passwordId) =>
            await _context.Passwords.AsNoTracking().FirstOrDefaultAsync(p => p.PasswordId == passwordId);

        public async Task<Password?> GetByUserIdAsync(Guid userId) =>
            await _context.Passwords
                .FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task<bool> UpdateAsync(Guid userId, string newPassword)
        {
            var password = await GetByUserIdAsync(userId)
                           ?? throw new KeyNotFoundException($"Password by user ID [{userId}] not found.");

            password.PasswordHash = PasswordExtensions.HashPassword(newPassword, password.PasswordSalt);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> VerifyAsync(Guid passwordId, string plainPassword)
        {
            var passwordEntity = await GetByIdAsync(passwordId);
            if (passwordEntity == null) return false;

            var hashedInput = PasswordExtensions.HashPassword(plainPassword, passwordEntity.PasswordSalt);
            return hashedInput == passwordEntity.PasswordHash;
        }

        public async Task<bool> AddAsync(Guid id, string password, Guid userId)
        {
            var salt = PasswordExtensions.GenerateSalt();
            var hash = PasswordExtensions.HashPassword(password, salt);

            var passwordEntity = new Password
            {
                PasswordId = id,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserId = userId
            };


            await _context.Passwords.AddAsync(passwordEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid passwordId)
        {
            try
            {
                var password = await _context.Passwords.FindAsync(passwordId);

                _context.Passwords.Remove(password);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Problem in PasswordRepository, method DeleteAsync: {ex}");
                return false;
            }
        }


        public async Task<string> GetHashByIdAsync(Guid passwordId)
        {
            return _context.Passwords.FirstOrDefaultAsync(p => p.PasswordId == passwordId).Result.PasswordHash;
        }
    }
}