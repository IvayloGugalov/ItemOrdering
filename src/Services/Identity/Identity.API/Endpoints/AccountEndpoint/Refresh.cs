using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.Domain;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

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

        [HttpPost(RefreshRequest.ROUTE)]
        public async Task<ActionResult> RefreshTokenAsync([FromBody] RefreshRequest refreshRequest)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var validationResult = this.refreshTokenValidator.Validate(refreshRequest.RefreshTokenValue);
            switch (validationResult)
            {
                case TokenValidationResult.TokenExpired:
                    return this.BadRequest(new ErrorResponse("Token has expired"));
                case TokenValidationResult.InvalidSignature:
                    return this.BadRequest(new ErrorResponse("Token has invalid signature"));
            }

            var refreshToken = await this.refreshTokenRepository.GetByTokenValueAsync(refreshRequest.RefreshTokenValue);
            if (refreshToken is null) return NotFound(new ErrorResponse("Invalid refresh token"));

            // Invalidate the refresh token
            await this.refreshTokenRepository.DeleteAsync(refreshToken.Id);

            var user = await this.userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user is null) return NotFound(new ErrorResponse("User not found"));

            var (newAccessToken, newRefreshToken) = await this.authenticator.AuthenticateUserAsync(user);
            var response = new UserAuthenticatedDto(newAccessToken, newRefreshToken);

            return Ok(response);
        }
    }
}
