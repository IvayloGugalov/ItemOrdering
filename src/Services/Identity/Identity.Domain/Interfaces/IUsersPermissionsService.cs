using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Domain.Interfaces
{
    public interface IUsersPermissionsService
    {
        List<string> PermissionsFromUser(ClaimsPrincipal user);
    }
}