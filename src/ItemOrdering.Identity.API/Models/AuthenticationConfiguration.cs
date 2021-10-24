namespace ItemOrdering.Identity.API.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecretKey { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public string RefreshTokenSecretKey { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
