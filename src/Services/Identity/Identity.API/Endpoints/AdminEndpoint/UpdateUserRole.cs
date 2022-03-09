using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
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
        public async Task<IActionResult> UpdateUserRoleAsync([FromBody] UpdateUserRoleRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var result = await this.adminUserService.UpdateUserRolesAsync(request.UserId, request.Role);

            return result ? Ok() : BadRequest();
        }
    }
}
