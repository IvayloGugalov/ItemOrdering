using System.Threading.Tasks;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.API.Models;
using Identity.API.Services.Repositories;
using Identity.API.Services.TokenGenerators;

namespace Identity.API.Services.Authenticators
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

        public async Task<LoginResponse> AuthenticateUserAsync(User user)
        {
            var accessTokenValue = this.accessTokenGenerator.GenerateAccessToken(user);
            var refreshTokenValue = this.refreshTokenGenerator.GenerateRefreshToken();

            var refreshToken = new RefreshToken(refreshTokenValue, user.Id);

            await this.refreshTokenRepository.CreateAsync(refreshToken);

            return new LoginResponse
            {
                UserAuthenticatedDto = new UserAuthenticatedDto(accessTokenValue, refreshTokenValue)
            };
        }
    }
}
