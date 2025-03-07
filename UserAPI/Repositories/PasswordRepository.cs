using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Data;
using UserAPI.Models;

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
        public PasswordRepository()
        {
            _context = null;

        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            var result = Convert.ToBase64String(hash);
            return result.Substring(0, result.Length - 1);
        }

        // size -> size % 8 == 0
        public string GenerateSalt(int size = 8)
        {
            if (size % 8 != 0)
            {
                _logger.LogInformation("Wrong size in GenerateSalt");
                return null;
            }

            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string result = Convert.ToBase64String(saltBytes);
            _logger.LogInformation("Successfull creation of salt");
            return result.Substring(0, result.Length - (size / 8));
        }

        public async Task<Password> GetByIdAsync(Guid passwordId)
        {
            return await _context.Passwords.FirstOrDefaultAsync(p => p.PasswordId == passwordId);
        }

        public async Task<bool> UpdateAsync(Guid userId, string oldPassword, string newPassword)
        {
            if (await VerifyAsync(userId, oldPassword))
            {
                var passwordEntity = await GetByIdAsync(userId);
                var newPasswordEntity = new Password
                {
                    PasswordId = passwordEntity.PasswordId,
                    PasswordHash = HashPassword(newPassword, passwordEntity.PasswordSalt),
                    PasswordSalt = passwordEntity.PasswordSalt,
                    UserId = userId
                };

                _context.Passwords.Update(newPasswordEntity);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> VerifyAsync(Guid passwordId, string plainPassword)
        {
            var passwordEntity = await GetByIdAsync(passwordId);
            if (passwordEntity == null) return false;

            var hashedInput = HashPassword(plainPassword, passwordEntity.PasswordSalt);
            return hashedInput == passwordEntity.PasswordHash;
        }

        public async Task<bool> AddAsync(string password, User user)
        {
            var salt = GenerateSalt();
            var hash = HashPassword(password, salt);

            var passwordEntity = new Password
            {
                PasswordId = Guid.NewGuid(),
                PasswordHash = hash,
                PasswordSalt = salt,
                User = user,
                UserId = user.UserId
            };


            await _context.AddAsync(passwordEntity);
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
            return  _context.Passwords.FirstOrDefaultAsync(p => p.PasswordId == passwordId).Result.PasswordHash;
        }
    }
}
