using System.Security.Claims;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IClaimsExtractor claimsExtractor;

        public AccessTokenGenerator(
            AuthenticationConfiguration authConfiguration,
            ITokenGenerator tokenGenerator,
            IClaimsExtractor claimsExtractor)
        {
            this.authConfiguration = authConfiguration;
            this.tokenGenerator = tokenGenerator;
            this.claimsExtractor = claimsExtractor;
        }

        public async Task<string> GenerateAccessToken(AuthUser user)
        {
            var claims = await this.claimsExtractor.GetClaimForAuthUser(user);

            claims.AddRange(new[]
            {
                new Claim(PermissionConstants.UserIdClaimType, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
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
