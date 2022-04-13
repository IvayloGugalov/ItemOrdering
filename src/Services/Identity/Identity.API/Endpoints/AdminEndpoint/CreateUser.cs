using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
using Identity.Permissions;
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

        [HttpPost(CreateUserRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateUserResponse>> RegisterUserAsync([FromBody] CreateUserRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminUserService.AddNewUser(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                userName: request.Username,
                password: request.Password,
                roleNames: request.Roles);

            if (result.HasErrors)
            {
                return Conflict(new ErrorResponse(result.GetErrorMessages()));
            }

            return Ok(new CreateUserResponse { User = result.Result.MapAuthUserToUserDto() });
        }
    }
}
