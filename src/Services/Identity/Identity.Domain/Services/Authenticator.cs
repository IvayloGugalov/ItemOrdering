using System;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAccessTokenGenerator accessTokenGenerator;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public Authenticator(IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
        {
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<AccessAndRefreshToken> AuthenticateUserAsync(AuthUser user)
        {
            var accessTokenValue = await this.accessTokenGenerator.GenerateAccessToken(user);
            var refreshTokenValue = await this.refreshTokenGenerator.GenerateRefreshToken(user.Id);

            return new AccessAndRefreshToken(accessTokenValue, refreshTokenValue);
        }
    }
}
