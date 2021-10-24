using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Identity.API.Models;
using ItemOrdering.Identity.API.Services.Authenticators;
using ItemOrdering.Identity.API.Services.Repositories;
using ItemOrdering.Identity.API.Services.TokenValidators;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Refresh : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IRefreshTokenValidator refreshTokenValidator;
        private readonly IAuthenticator authenticator;
        private readonly UserManager<User> userManager;

        public Refresh(
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenValidator refreshTokenValidator,
            IAuthenticator authenticator,
            UserManager<User> userManager)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.refreshTokenValidator = refreshTokenValidator;
            this.authenticator = authenticator;
            this.userManager = userManager;
        }

        [HttpPost(RefreshRequest.ROUTE)]
        public async Task<ActionResult> RefreshTokenWithManagerAsync([FromBody]RefreshRequest refreshRequest)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            if (!this.refreshTokenValidator.Validate(refreshRequest.RefreshTokenValue)) return this.BadRequest(new ErrorResponse("Invalid refresh token"));

            // TODO: Expired? Invalid signature? Handle cases
            var refreshToken = await this.refreshTokenRepository.GetByTokenValue(refreshRequest.RefreshTokenValue);
            if (refreshToken is null) return NotFound(new ErrorResponse("Invalid refresh token"));

            // Invalidate the refresh token
            await this.refreshTokenRepository.DeleteAsync(refreshToken.Id);

            var user = await this.userManager.FindByIdAsync(refreshToken.UserId.ToString());
            if (user is null) return NotFound(new ErrorResponse("User not found"));

            var response = await this.authenticator.AuthenticateUserAsync(user);
            return Ok(response);
        }
    }
}
