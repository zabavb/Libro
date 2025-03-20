namespace UserAPI.Models.Auth
{
    public class OAuthRequest
    {
        public string Token { get; set; }

        public OAuthRequest() => Token = string.Empty;
    }
}
