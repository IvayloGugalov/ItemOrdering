using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAccessTokenGenerator accessTokenGenerator;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public Authenticator(
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticationTokens> AuthenticateUserAsync(AuthUser user)
        {
            var accessTokenValue = this.accessTokenGenerator.GenerateAccessToken(user);
            var refreshTokenValue = this.refreshTokenGenerator.GenerateRefreshToken();

            var refreshToken = new RefreshToken(refreshTokenValue, user.Id);

            await this.refreshTokenRepository.CreateAsync(refreshToken);

            return new AuthenticationTokens(accessTokenValue, refreshTokenValue);
        }
    }
}
