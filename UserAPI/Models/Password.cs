using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models
{
    public class Password
    {
        public Guid PasswordId { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Password()
        {
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
        }
    }
}
