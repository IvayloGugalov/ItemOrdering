using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Identity.API.Models;

namespace Identity.API.Services.TokenValidators
{
    public class RefreshTokenValidator : IRefreshTokenValidator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ILogger<RefreshTokenValidator> logger;

        public RefreshTokenValidator(AuthenticationConfiguration authConfiguration, ILogger<RefreshTokenValidator> logger)
        {
            this.authConfiguration = authConfiguration;
            this.logger = logger;
        }

        public bool Validate(string refreshTokenValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.authConfiguration.RefreshTokenSecretKey)),
                ValidIssuer = this.authConfiguration.Issuer,
                ValidAudience = this.authConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(refreshTokenValue, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Could not validate refresh token.");
                return false;
            }
        }
    }
}
