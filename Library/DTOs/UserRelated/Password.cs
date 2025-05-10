namespace Library.DTOs.UserRelated
{
    public class Password
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public Password()
        {
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
        }
    }
}
