using System;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Tokens;
using Identity.Tokens.Interfaces;

namespace Identity.Domain.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAccessTokenGenerator accessTokenGenerator;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly IClaimsExtractor claimsExtractor;

        public Authenticator(
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IClaimsExtractor claimsExtractor)
        {
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.claimsExtractor = claimsExtractor;
        }

        public async Task<AccessAndRefreshToken> AuthenticateUserAsync(AuthUser user)
        {
            var accessTokenValue = await this.RefreshAccessToken(user);
            var refreshTokenValue = await this.RefreshRefreshToken(user.Id);

            return new AccessAndRefreshToken(accessTokenValue, refreshTokenValue);
        }

        public async Task<string> RefreshAccessToken(AuthUser user)
        {
            var accessTokenValue = await this.accessTokenGenerator.GenerateAccessToken(
                () => this.claimsExtractor.GetClaimForAuthUser(user),
                user.Id,
                userEmail: user.Email,
                userName: user.UserName);

            return accessTokenValue;
        }

        public async Task<string> RefreshRefreshToken(Guid userId)
        {
            var refreshTokenValue = await this.refreshTokenGenerator.GenerateRefreshToken(userId);

            return refreshTokenValue;
        }
    }
}
