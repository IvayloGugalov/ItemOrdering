using System.Collections.Generic;
using System.Security.Claims;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ITokenGenerator tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration authConfiguration, ITokenGenerator tokenGenerator)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
        }

        public string GenerateAccessToken(AuthUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return this.tokenGenerator.GenerateToken(
                secretKey: this.authConfiguration.AccessTokenSecretKey,
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                tokenExpirationMinutes: this.authConfiguration.AccessTokenExpirationMinutes,
                claims: claims);
        }
    }
}
