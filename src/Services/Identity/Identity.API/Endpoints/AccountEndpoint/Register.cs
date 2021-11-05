using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            // TODO: Validate that the passed role is real
            var (roleName, roleDescription) = request.Role.GetAttributeInfo();
            if (roleDescription == string.Empty) return BadRequest(new ErrorResponse("Role is invalid"));

            var customerPermission = request.Role.GetPermissionAsChar();
            var roleToPermissions = new RoleToPermissions(roleName, roleDescription, customerPermission.ToString());

            var result = await this.userService.RegisterUserAsync(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                userName: request.Username,
                password: request.Password,
                roleToPermissions: roleToPermissions);

            if (!result.Succeeded)
            {
                IdentityErrorDescriber errorDescriber;
                var error = result.Errors.FirstOrDefault();

                return error?.Code switch
                {
                    nameof(errorDescriber.DuplicateEmail) => Conflict(new ErrorResponse("Email already exists.")),
                    nameof(errorDescriber.DuplicateUserName) => Conflict(new ErrorResponse("UserName already exists.")),
                    _ => Conflict(new ErrorResponse(error.Code)),
                };
            }

            return Ok(result);
        }
    }
}
