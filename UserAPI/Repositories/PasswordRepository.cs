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

        public static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            var result = Convert.ToBase64String(hash);
            return result.Substring(0, result.Length - 1);
        }

        // size -> size % 8 == 0
        public static string? GenerateSalt(int size = 8)
        {
            if (size % 8 != 0)
                return null;

            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string result = Convert.ToBase64String(saltBytes);
            return result.Substring(0, result.Length - (size / 8));
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

            password.PasswordHash = HashPassword(newPassword, password.PasswordSalt);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> VerifyAsync(Guid passwordId, string plainPassword)
        {
            var passwordEntity = await GetByIdAsync(passwordId);
            if (passwordEntity == null) return false;

            var hashedInput = HashPassword(plainPassword, passwordEntity.PasswordSalt);
            return hashedInput == passwordEntity.PasswordHash;
        }

        public async Task<bool> AddAsync(Guid id, string password, Guid userId)
        {
            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);

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