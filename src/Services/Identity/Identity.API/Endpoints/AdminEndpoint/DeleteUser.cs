using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
using Identity.Permissions;
using Identity.Shared;

namespace Identity.API.Endpoints.AdminEndpoint
{
    [ApiController]
    public class DeleteUser : ControllerBase
    {
        private readonly IAdminUserService adminUserService;

        public DeleteUser(IAdminUserService adminUserService)
        {
            this.adminUserService = adminUserService;
        }

        [HttpDelete(DeleteUserRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var currentUserEmail = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (currentUserEmail == request.Email) return BadRequest(new ErrorResponse("Can't delete yourself!"));

            var result = await this.adminUserService.DeleteAuthUserAsync(request.Email);

            if (result.HasErrors)
            {
                return BadRequest(new ErrorResponse(result.GetErrorMessages()));
            }

            return Ok();
        }
    }
}