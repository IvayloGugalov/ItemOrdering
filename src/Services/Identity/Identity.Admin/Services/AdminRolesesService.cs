using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GenericStatus;
using Microsoft.AspNetCore.Identity;

using Identity.Admin.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Permissions;

namespace Identity.Admin.Services
{
    public class AdminRolesesService : IAdminRolesService
    {
        private readonly IRoleToPermissionRepository rolesToPermissionsRepository;
        private readonly IUserToRoleRepository usersToRolesRepository;
        private readonly UserManager<AuthUser> userManager;

        public AdminRolesesService(IRoleToPermissionRepository rolesToPermissionsRepository, IUserToRoleRepository usersToRolesRepository, UserManager<AuthUser> userManager)
        {
            this.rolesToPermissionsRepository = rolesToPermissionsRepository;
            this.usersToRolesRepository = usersToRolesRepository;
            this.userManager = userManager;
        }

        public IQueryable<RoleToPermissions> QueryRoleToPermissions()
        {
            return this.rolesToPermissionsRepository.QueryRoleToPermissions();
        }

        public async Task<bool> IsRoleNameExistingAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

            return await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName) != null;
        }

        public IQueryable<AuthUser> QueryUsersByRole(string roleName)
        {
            return this.userManager.Users.Where(x => x.UserRoles.Any(y => y.RoleName == roleName));
        }

        public async Task<IGenericStatus<RoleToPermissions>> CreateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            if (permissionNames == null) throw new ArgumentNullException(nameof(permissionNames));

            var status = new GenericStatus<RoleToPermissions>();

            if (await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName) != null) return status.AddError($"Role {roleName} already exists");

            // The permissions set on the role must match the existing permission types in Permissions.Permissions
            var packedPermissions = permissionNames.GetPackedPermissionsFromEnumerable();

            if (!packedPermissions.Any()) return status.AddError($"None of the passed permission names match the permissions set in {nameof(Permissions.Permissions)}");

            var role = new RoleToPermissions(roleName, description, packedPermissions);

            await this.rolesToPermissionsRepository.CreateAsync(role);

            return status.SetResult(role);
        }

        // TODO: Decide whether to merge the old information with the new one
        public async Task<IGenericStatus<RoleToPermissions>> UpdateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            if (permissionNames == null) throw new ArgumentNullException(nameof(permissionNames));

            var status = new GenericStatus<RoleToPermissions>();

            var existingRolePermission = await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName);
            if (existingRolePermission == null) return status.AddError($"Role {roleName} does not exist");

            var packedPermissions = permissionNames.GetPackedPermissionsFromEnumerable();

            if (!packedPermissions.Any()) return status.AddError("None of the passed permissions exist in the database");

            existingRolePermission.Update(packedPermissions, description);
            await this.rolesToPermissionsRepository.UpdateAsync(existingRolePermission);

            status.SetResult(existingRolePermission);
            return status;
        }

        // TODO: Need confirmation to remove role from users - removeFromUsers
        public async Task<IGenericStatus<bool>> DeleteRoleAsync(string roleName, bool removeFromUsers)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

            var status = new GenericStatus<bool>();

            var existingRolePermission = await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName);
            if (existingRolePermission == null) return status.AddError($"Role {roleName} does not exist");

            var usersWithRole = (await this.usersToRolesRepository.GetRolesAsync())
                .Where(x => x.RoleName == roleName)
                .ToList();

            var deleted = false;
            if (usersWithRole.Any())
            {
                if (!removeFromUsers) return status.AddError($"Can't delete {roleName} from {usersWithRole.Count} users"); ;

                deleted = await this.usersToRolesRepository.DeleteManyByIdAsync(usersWithRole.Select(y => y.Id));
            }

            deleted |= await this.usersToRolesRepository.DeleteByRoleNameAsync(roleName);

            return deleted
                ? status.SetResult(true)
                : status.AddError("Delete failed.");
        }
    }
}
