using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Identity.Domain;
using Identity.Domain.Interfaces;

using TokenValidationResult = Identity.Domain.TokenValidationResult;

namespace Identity.API.Services.TokenServices.TokenValidators
{
    // TODO: Merge with RefreshTokenValidator?
    public class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ILogger<AccessTokenValidator> logger;

        public AccessTokenValidator(ILogger<AccessTokenValidator> logger, AuthenticationConfiguration authConfiguration)
        {
            this.logger = logger;
            this.authConfiguration = authConfiguration;
        }

        public TokenValidationResult Validate(string accessTokenValue, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.authConfiguration.AccessTokenSecretKey)),
                ValidIssuer = this.authConfiguration.Issuer,
                ValidAudience = this.authConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(accessTokenValue, validationParameters, out var validatedToken);
                return TokenValidationResult.Success;
            }
            catch (SecurityTokenExpiredException stee)
            {
                this.logger.LogWarning(stee, "Token has expired.");
                return TokenValidationResult.TokenExpired;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error validating the token.");
                return TokenValidationResult.Unknown;
            }
        }
    }
}
