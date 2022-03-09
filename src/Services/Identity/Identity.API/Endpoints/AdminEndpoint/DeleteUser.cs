using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
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
        public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var isDeleted = await this.adminUserService.DeleteAuthUserAsync(request.UserId);

            return isDeleted ? Ok() : BadRequest("Unable to delete user");
        }
    }
}
