using GuardClauses;

namespace Identity.Domain
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecretKey { get; set; }
        public string RefreshTokenSecretKey { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        /// <summary>
        /// Checks that the properties have been set
        /// </summary>
        public void CheckJwtConfiguration()
        {
            Guard.Against.NullOrEmpty(this.AccessTokenSecretKey, nameof(this.AccessTokenSecretKey));
            Guard.Against.NullOrEmpty(this.RefreshTokenSecretKey, nameof(this.RefreshTokenSecretKey));
            Guard.Against.NegativeOrZero(this.AccessTokenExpirationMinutes, nameof(this.AccessTokenExpirationMinutes));
            Guard.Against.NegativeOrZero(this.RefreshTokenExpirationMinutes, nameof(this.RefreshTokenExpirationMinutes));
            Guard.Against.NullOrEmpty(this.Issuer, nameof(this.Issuer));
            Guard.Against.NullOrEmpty(this.Audience, nameof(this.Audience));
        }
    }
}
