using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Identity.Permissions;
using Identity.Tokens.Interfaces;

namespace Identity.Tokens.TokenGenerators
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ITokenGenerator tokenGenerator;

        public AccessTokenGenerator(
            AuthenticationConfiguration authConfiguration,
            ITokenGenerator tokenGenerator)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<string> GenerateAccessToken(Func<Task<List<Claim>>> getClaimsFunc, Guid userId, string userEmail, string userName)
        {
            var claims = await getClaimsFunc();

            claims.AddRange(new[]
            {
                new Claim(PermissionConstants.UserIdClaimType, userId.ToString()),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Name, userName)
            });

            return this.tokenGenerator.GenerateToken(
                secretKey: this.authConfiguration.AccessTokenSecretKey,
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                tokenExpirationMinutes: this.authConfiguration.AccessTokenExpirationMinutes,
                claims: claims);
        }
    }
}
