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
    public class UpdateUserRole : ControllerBase
    {
        private readonly IAdminUserService adminUserService;

        public UpdateUserRole(IAdminUserService adminUserService)
        {
            this.adminUserService = adminUserService;
        }

        [HttpPost(UpdateUserRoleRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<ActionResult<UpdateUserRoleResponse>> UpdateUserRoleAsync([FromBody] UpdateUserRoleRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminUserService.UpdateUserRolesAsync(request.UserId, request.Role);

            if (result.HasErrors) return BadRequest(new ErrorResponse(result.GetErrorMessages()));

            return Ok(new UpdateUserRoleResponse { User = result.Result.MapAuthUserToUserDto() });
        }
    }
}
