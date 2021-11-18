using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Identity.Tokens.Interfaces;

namespace Identity.Tokens.TokenValidators
{
    // TODO: Merge with AccessTokenValidator?
    public class RefreshTokenValidator : IRefreshTokenValidator
    {
        private readonly AuthenticationConfiguration authConfiguration;
        private readonly ILogger<RefreshTokenValidator> logger;

        public RefreshTokenValidator(AuthenticationConfiguration authConfiguration, ILogger<RefreshTokenValidator> logger)
        {
            this.authConfiguration = authConfiguration;
            this.logger = logger;
        }

        public TokenValidationResult Validate(string refreshTokenValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
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
                return TokenValidationResult.Success;
            }
            catch (SecurityTokenExpiredException stee)
            {
                this.logger.LogWarning(stee, "Token has expired.");
                return TokenValidationResult.TokenExpired;
            }
            catch (SecurityTokenInvalidSignatureException stise)
            {
                this.logger.LogWarning(stise, "Token has invalid signature.");
                return TokenValidationResult.InvalidSignature;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error validating the token.");
                return TokenValidationResult.Unknown;
            }
        }
    }
}
