using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Services.Repositories;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Logout : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public Logout(IRefreshTokenRepository refreshTokenRepository)
        {
            this.refreshTokenRepository = refreshTokenRepository;
        }

        [HttpDelete(LogoutRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogoutAsync()
        {
            var unparsedUserId = HttpContext.User.FindFirstValue("id");

            if (!Guid.TryParse(unparsedUserId, out var userId)) return Unauthorized();

            await this.refreshTokenRepository.DeleteAllForUserAsync(userId);

            return NoContent();
        }
    }
}
