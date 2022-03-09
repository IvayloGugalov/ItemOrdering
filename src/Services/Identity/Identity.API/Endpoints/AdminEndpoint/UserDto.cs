using System.Collections.Generic;
using System.Linq;

using Identity.Domain.Entities;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public record UserDto(string FirstName, string LastName, string Email, string UserName, IEnumerable<string> Roles);

    public static class Mapper
    {
        public static UserDto MapAuthUserToUserDto(this AuthUser authUser)
        {
            return new UserDto(
                FirstName: authUser.FirstName,
                LastName: authUser.LastName,
                Email: authUser.Email,
                UserName: authUser.UserName,
                Roles: authUser.UserRoles.Select(x => x.RoleName));
        }
    }
}
