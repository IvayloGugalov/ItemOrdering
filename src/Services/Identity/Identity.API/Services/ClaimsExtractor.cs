using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Permissions;

namespace Identity.API.Services
{
    public class ClaimsExtractor : IClaimsExtractor
    {
        private readonly IUserToRoleRepository userToRoleRepository;

        public ClaimsExtractor(IUserToRoleRepository userToRoleRepository)
        {
            this.userToRoleRepository = userToRoleRepository;
        }

        public async Task<List<Claim>> GetClaimForAuthUser(AuthUser user)
        {
            var claims = new List<Claim>();
            var permissions = await this.GetPermissionsForUser(user.Id);

            if (permissions != null)
            {
                claims.Add(new Claim(PermissionConstants.PackedPermissionClaimType, permissions));
            }
            // TODO: Maybe this isn't needed
            else
            {
                var roles = user.UserRoles.Select(x =>
                    (char)Convert.ChangeType(Enum.Parse(typeof(Permissions.Permissions), x.RoleName), typeof(char)));

                var allRoles = new string(roles.ToArray());

                claims.Add(new Claim(PermissionConstants.PackedPermissionClaimType, allRoles));
            }

            return claims;
        }

        private async Task<string> GetPermissionsForUser(Guid userId)
        {
            var permissionsForUser = (await this.userToRoleRepository.GetRolesForAuthUserAsync(userId))
                .Select(x => x.Role.PackedPermissionsInRole)
                .ToArray();

            if (!permissionsForUser.Any()) return null;

            var packedPermissions = new string(string.Concat(permissionsForUser).Distinct().ToArray());

            return packedPermissions;
        }
    }
}
