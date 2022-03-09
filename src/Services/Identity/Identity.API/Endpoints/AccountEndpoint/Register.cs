using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Permissions;
using Identity.Shared;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Register : ControllerBase
    {
        private readonly IUserService userService;

        public Register(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost(RegisterRequest.ROUTE)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterWithManagerAsync([FromBody] RegisterRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            // TODO: Validate that the passed role is real
            var roleToPermissions = new RoleToPermissions(request.Role);

            var result = await this.userService.RegisterUserAsync(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                userName: request.Username,
                password: request.Password,
                roleToPermissions: roleToPermissions);

            if (!result.Succeeded)
            {
                return Conflict(result.GetErrorResponse());
            }

            return Ok(result);
        }
    }
}
