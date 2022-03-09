using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Identity.Admin.Interfaces;

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
        public async Task<IActionResult> GetUsersAsync()
        {
            if (!this.ModelState.IsValid) return null;

            var users = await Task.Run(() => this.adminUserService.QueryAuthUsersAsync());

            var userDtos = new List<UserDto>();
            foreach (var authUser in users)
            {
                userDtos.Add(new UserDto(authUser.FirstName, authUser.LastName, authUser.Email, authUser.UserName, authUser.UserRoles.Select(x => x.RoleName)));
            }

            return Ok(userDtos);
        }
    }
}
