namespace UserAPI.Models.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }

        public JwtSettings()
        {
            SecretKey = null!;
            Issuer = null!;
            Audience = null!;
        }
    }
}
