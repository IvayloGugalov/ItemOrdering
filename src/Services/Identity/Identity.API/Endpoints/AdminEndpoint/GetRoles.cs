using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Identity.Admin.Interfaces;
using Identity.Permissions;
using Identity.Shared;

namespace Identity.API.Endpoints.AdminEndpoint
{
    [ApiController]
    public class GetRoles : ControllerBase
    {
        private readonly IAdminRoleService adminRoleService;

        public GetRoles(IAdminRoleService adminRoleService)
        {
            this.adminRoleService = adminRoleService;
        }

        [HttpGet(GetRolesRequest.ROUTE)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRolesAsync()
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var roles = await Task.Run(() => this.adminRoleService.QueryRoleToPermissions());
            var roleNames = roles.ToList()
                .Where(x => x.RoleName != Permissions.Permissions.NotSet.GetDisplayName())
                .Select(x => Enum.Parse<Permissions.Permissions>(x.RoleName).GetDisplayName());

            var result = JsonConvert.SerializeObject(roleNames);

            return Ok(result);
        }
    }
}
