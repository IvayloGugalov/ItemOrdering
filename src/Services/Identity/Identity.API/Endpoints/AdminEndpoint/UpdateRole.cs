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
    public class UpdateRole : ControllerBase
    {
        private readonly IAdminRolesService adminRolesService;

        public UpdateRole(IAdminRolesService adminRolesService)
        {
            this.adminRolesService = adminRolesService;
        }

        [HttpPut(UpdateRoleRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<ActionResult<UpdateRoleResponse>> UpdateRoleAsync([FromBody] UpdateRoleRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminRolesService.UpdateRoleToPermissionsAsync(
                roleName: request.RoleName,
                permissionNames: request.PermissionNames,
                description: request.Description);

            return Ok(new UpdateRoleResponse { RoleToPermission = new RoleToPermissionDto(result.Result.DisplayName, result.Result.PackedPermissionsInRole) });
        }
    }
}
