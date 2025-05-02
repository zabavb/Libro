using System.Security.Cryptography;
using System.Text;

namespace UserAPI.Models
{
    public static class PasswordExtensions
    {
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
    }
}