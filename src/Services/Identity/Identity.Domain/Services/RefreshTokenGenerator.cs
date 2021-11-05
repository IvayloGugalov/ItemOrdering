using System;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    /// <summary>
    /// Generator class for a Refresh Token.
    /// Authorize with a Refresh Token so the user can receive a new Access Token when it has expired.
    /// </summary>
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public RefreshTokenGenerator(
            AuthenticationConfiguration authConfiguration,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var refreshTokenValue = this.tokenGenerator.GenerateToken(
                secretKey: this.authConfiguration.RefreshTokenSecretKey,
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                tokenExpirationMinutes: this.authConfiguration.RefreshTokenExpirationMinutes);

            var refreshToken = new RefreshToken(refreshTokenValue, userId);

            await this.refreshTokenRepository.CreateAsync(refreshToken);

            return refreshTokenValue;
        }
    }
}
