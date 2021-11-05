using System.Collections.Generic;
using System.Security.Claims;

using Identity.API.Policy;
using Identity.Domain;
using Identity.Domain.Interfaces;

namespace Identity.API.Services
{
    public class UsersPermissionsService : IUsersPermissionsService
    {
        private readonly AuthPermissionsOptions options;

        public UsersPermissionsService(AuthPermissionsOptions options)
        {
            this.options = options;
        }

        // for ui visualization
        public List<string> PermissionsFromUser(ClaimsPrincipal user)
        {
            var packedPermissions = user.GetPackedPermissionsFromUser();

            return packedPermissions.ConvertPackedPermissionToNames(this.options.EnumPermissionsType);
        }
    }
}
