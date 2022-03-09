using System.Collections.Generic;
using System.Security.Claims;

using Identity.Permissions;
using Identity.Permissions.Interfaces;

namespace Identity.API.Services
{
    public class UsersPermissionsService : IUsersPermissionsService
    {
        // for ui visualization
        public List<string> GetPermissionsFromUser(ClaimsPrincipal user)
        {
            var packedPermissions = user.GetPackedPermissionsFromUser();

            return packedPermissions.ConvertPackedPermissionsToNames();
        }
    }
}
