﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Identity.API.Models;
using ItemOrdering.Identity.API.Services.Repositories;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Delete : ControllerBase
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly UserManager<User> userManager;

        public Delete(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            this.userManager = userManager;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        [HttpDelete(DeleteRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteWithManagerAsync()
        {
            var unparsedUserId = HttpContext.User.FindFirstValue("id");

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
