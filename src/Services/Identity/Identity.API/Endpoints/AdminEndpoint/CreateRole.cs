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
    public class CreateRole : ControllerBase
    {
        private readonly IAdminRolesService adminRolesService;

        public CreateRole(IAdminRolesService adminRolesService)
        {
            this.adminRolesService = adminRolesService;
        }

        [HttpPost(CreateRoleRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<ActionResult<CreateRoleResponse>> CreateRoleAsync([FromBody] CreateRoleRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminRolesService.CreateRoleToPermissionsAsync(
                roleName: request.RoleName,
                permissionNames: request.PermissionNames,
                description: request.Description);

            if (result.HasErrors)
            {
                return Conflict(new ErrorResponse(result.GetErrorMessages()));
            }

            return Ok(new CreateRoleResponse { RoleToPermission = new RoleToPermissionDto(result.Result.DisplayName, result.Result.PackedPermissionsInRole) });
        }
    }
}
