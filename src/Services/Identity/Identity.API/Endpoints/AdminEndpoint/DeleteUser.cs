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

        [HttpPost(DeleteUserRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var currentUserEmail = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (currentUserEmail == request.Email) return BadRequest("Can't delete yourself!");

            var isDeleted = await this.adminUserService.DeleteAuthUserAsync(request.Email);

            return isDeleted ? Ok() : BadRequest("Unable to delete user");
        }
    }
}