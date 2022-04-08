using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

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
            var result = await this.userService.RegisterUserAsync(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                userName: request.Username,
                password: request.Password,
                roleToPermissions: new RoleToPermissions(Permissions.Permissions.Customer));

            if (!result.Succeeded)
            {
                return Conflict(result.GetErrorResponse());
            }

            return Ok(result);
        }
    }
}
