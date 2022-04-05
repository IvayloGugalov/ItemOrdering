using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Shared;
using Identity.Tokens;
using Identity.Tokens.Interfaces;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Refresh : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IRefreshTokenValidator refreshTokenValidator;
        private readonly IAuthenticator authenticator;
        private readonly UserManager<AuthUser> userManager;

        public Refresh(
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenValidator refreshTokenValidator,
            IAuthenticator authenticator,
            UserManager<AuthUser> userManager)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.refreshTokenValidator = refreshTokenValidator;
            this.authenticator = authenticator;
            this.userManager = userManager;
        }

        [HttpGet(RefreshRequest.ROUTE)]
        public async Task<ActionResult> RefreshTokenAsync()
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var (_, refreshTokenValue) = this.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == AppendCookieExtension.REFRESH_TOKEN_NAME);
            if (string.IsNullOrEmpty(refreshTokenValue)) return Unauthorized(new ErrorResponse("No refresh token found"));

            var refreshTokenValidationResult = this.refreshTokenValidator.Validate(refreshTokenValue);
            switch (refreshTokenValidationResult)
            {
                case TokenValidationResult.InvalidSignature:
                    return Unauthorized(new ErrorResponse("Token has invalid signature"));
                case TokenValidationResult.EncryptionKeyNotFound:
                    return Unauthorized(new ErrorResponse("No encryption on token"));
                case TokenValidationResult.Unknown:
                    return Unauthorized(new ErrorResponse("Unknown error"));
            }

            var refreshToken = await this.refreshTokenRepository.GetByTokenValueAsync(refreshTokenValue);
            if (refreshToken is null) return Unauthorized(new ErrorResponse("No refresh token found"));

            var user = await this.userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user is null) return NotFound(new ErrorResponse("User not found"));

            var roles = string.Concat(user.UserRoles.Select(x => x.Role.PackedPermissionsInRole));

            if (refreshTokenValidationResult == TokenValidationResult.Success)
            {
                var newAccessToken = await this.authenticator.RefreshAccessToken(user);
                var response = new UserAuthenticatedDto(
                    AccessToken: newAccessToken,
                    Roles: roles);

                return Ok(response);
            }
            else
            {
                // Invalidate the refresh token
                await this.refreshTokenRepository.DeleteAsync(refreshToken.Id);

                var (newAccessToken, newRefreshToken) = await this.authenticator.AuthenticateUserAsync(user);
                var response = new UserAuthenticatedDto(
                    AccessToken: newAccessToken,
                    Roles: roles);

                this.HttpContext.Response.Cookies.AppendRefreshToken(newRefreshToken);
                return Ok(response);
            }
        }
    }
}
