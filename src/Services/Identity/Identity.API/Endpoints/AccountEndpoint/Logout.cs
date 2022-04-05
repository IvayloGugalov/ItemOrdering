using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Permissions;
using Identity.Tokens.Interfaces;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Logout : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly SignInManager<AuthUser> signInManager;

        public Logout(IRefreshTokenRepository refreshTokenRepository, SignInManager<AuthUser> signInManager)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.signInManager = signInManager;
        }

        [HttpDelete(LogoutRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogoutAsync()
        {
            var unparsedUserId = this.HttpContext.User.FindFirstValue(PermissionConstants.UserIdClaimType);

            if (!Guid.TryParse(unparsedUserId, out var userId)) return Unauthorized();

            await this.refreshTokenRepository.DeleteAllForUserAsync(userId);
            await this.signInManager.SignOutAsync();

            this.HttpContext.Response.Cookies.Delete(AppendCookieExtension.REFRESH_TOKEN_NAME);

            return NoContent();
        }
    }
}