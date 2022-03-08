using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Permissions.Interfaces
{
    public interface IUsersPermissionsService
    {
        List<string> GetPermissionsFromUser(ClaimsPrincipal user);
    }
}
