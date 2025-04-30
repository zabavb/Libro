namespace UserAPI.Models
{
    public class Password
    {
        public Guid PasswordId { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Password(string passwordHash, string passwordSalt, Guid userId)
        {
            PasswordId = Guid.NewGuid();
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            UserId = userId;
            User = null!;
        }

        public Password()
        {
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
            User = null!;
        }
    }
}