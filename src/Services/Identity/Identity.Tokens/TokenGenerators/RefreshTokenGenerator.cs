using System;
using System.Security.Claims;
using System.Threading.Tasks;

using GuidGenerator;

using Identity.Permissions;
using Identity.Tokens.Interfaces;
using Identity.Tokens.Tokens;

namespace Identity.Tokens.TokenGenerators
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
        private readonly IGuidGeneratorService guidGenerator;

        public RefreshTokenGenerator(
            AuthenticationConfiguration authConfiguration,
            ITokenGenerator tokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IGuidGeneratorService guidGenerator)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
            this.refreshTokenRepository = refreshTokenRepository;
            this.guidGenerator = guidGenerator;
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var refreshTokenValue = this.tokenGenerator.GenerateToken(
                secretKey: this.authConfiguration.RefreshTokenSecretKey,
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                tokenExpirationMinutes: this.authConfiguration.RefreshTokenExpirationMinutes,
                claims: new[] { new Claim(PermissionConstants.UserIdClaimType, userId.ToString()), });

            var refreshToken = new RefreshToken(refreshTokenValue, userId, this.guidGenerator);

            await this.refreshTokenRepository.CreateAsync(refreshToken);

            return refreshTokenValue;
        }
    }
}
