using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
using Identity.Shared;

namespace Identity.API.Endpoints.AdminEndpoint
{
    [ApiController]
    public class CreateUser : ControllerBase
    {
        private readonly IAdminUserService adminUserService;

        public CreateUser(IAdminUserService adminUserService)
        {
            this.adminUserService = adminUserService;
        }

        // TODO: [HasPermission(Permissions.Permissions.Admin)]
        [HttpPost(CreateUserRequest.ROUTE)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] CreateUserRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminUserService.AddNewUser(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                userName: request.Username,
                password: request.Password,
                roleNames: request.Roles);

            if (result != null)
            {
                return Conflict(result);
            }

            return Ok();
        }
    }
}
