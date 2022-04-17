using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.Domain.Entities;
using Identity.Permissions;
using Identity.Shared;
using Identity.Tokens.Interfaces;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Delete : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly UserManager<AuthUser> userManager;

        public Delete(UserManager<AuthUser> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            this.userManager = userManager;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        [HttpDelete(DeleteRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAsync()
        {
            var unparsedUserId = this.HttpContext.User.FindFirstValue(PermissionConstants.UserIdClaimType);

            if (!Guid.TryParse(unparsedUserId, out var userId)) return Unauthorized();

            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user is null) return BadRequest(new ErrorResponse("No such user"));

            var result = await this.userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault();
                var message = error != null
                    ? error.Code
                    : "Unable to delete user";

                return Conflict(new ErrorResponse(message));
            }

            await this.refreshTokenRepository.DeleteAllForUserAsync(userId);

            return Ok(result);
        }
    }
}
