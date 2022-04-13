using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;
using Identity.Permissions;

namespace Identity.API.Endpoints.AdminEndpoint
{
    [ApiController]
    public class GetUsers : ControllerBase
    {
        private readonly IAdminUserService adminUserService;

        public GetUsers(IAdminUserService adminUserService)
        {
            this.adminUserService = adminUserService;
        }

        [HttpGet(GetUsersRequest.ROUTE)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Admin, Permissions.Permissions.SuperAdmin)]
        public async Task<ActionResult<List<UserDto>>> GetUsersAsync()
        {
            if (!this.ModelState.IsValid) return null;

            var usersQuery = await Task.Run(() => this.adminUserService.QueryAuthUsersAsync());

            var response = usersQuery.ToList().Select(user => user.MapAuthUserToUserDto());

            return Ok(response);
        }
    }
}