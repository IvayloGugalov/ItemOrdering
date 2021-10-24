using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Services.TokenGenerators
{
    /// <summary>
    /// Generator class for a Refresh Token.
    /// Authorize with a Refresh Token so the user can receive a new Access Token when the it has expired.
    /// </summary>
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ITokenGenerator tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration authConfiguration, ITokenGenerator tokenGenerator)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
        }

        public string GenerateRefreshToken()
        {
            return this.tokenGenerator.GenerateToken(
                secretKey: this.authConfiguration.RefreshTokenSecretKey,
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                tokenExpirationMinutes: this.authConfiguration.RefreshTokenExpirationMinutes);
        }
    }
}
