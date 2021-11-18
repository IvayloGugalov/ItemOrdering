using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Identity.Admin.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Admin.Services
{
    public class AdminRoleService : IAdminRoleService
    {
        private readonly IRoleToPermissionRepository rolesToPermissionsRepository;
        private readonly IUserToRoleRepository usersToRolesRepository;
        private readonly UserManager<AuthUser> userManager;

        public AdminRoleService(IRoleToPermissionRepository rolesToPermissionsRepository, IUserToRoleRepository usersToRolesRepository, UserManager<AuthUser> userManager)
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
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            return await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName) != null;
        }

        public IQueryable<AuthUser> QueryUsersUsingThisRole(string roleName)
        {
            return this.userManager.Users.Where(x => x.UserRoles.Any(y => y.RoleName == roleName));
        }

        public async Task<bool> CreateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null)
        {
            if (string.IsNullOrEmpty(roleName)) return false;
            if (await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName) != null) return false;
            if (permissionNames == null) return false;

            var packedPermissions = "";
            foreach (var permissionName in permissionNames)
            {
                if (Enum.TryParse(typeof(Permissions.Permissions), permissionName, ignoreCase: true, out _))
                {
                    packedPermissions += (char)Convert.ChangeType(permissionName, typeof(char));
                }
            }

            if (!packedPermissions.Any()) return false;

            await this.rolesToPermissionsRepository.CreateAsync(new RoleToPermissions(roleName, description, packedPermissions));

            return true;
        }

        public async Task<bool> UpdateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null)
        {
            var existingRolePermission = await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName);

            if (existingRolePermission == null) return false;

            var packedPermissions = "";
            foreach (var permissionName in permissionNames)
            {
                if (Enum.TryParse(typeof(Permissions.Permissions), permissionName, ignoreCase: true, out _))
                {
                    packedPermissions += (char)Convert.ChangeType(permissionName, typeof(char));
                }
            }

            if (!packedPermissions.Any()) return false;

            existingRolePermission.Update(packedPermissions, description);
            await this.rolesToPermissionsRepository.UpdateAsync(existingRolePermission);

            return true;
        }

        // TODO: Need confirmation to remove role from users - removeFromUsers
        public async Task<bool> DeleteRoleAsync(string roleName, bool removeFromUsers)
        {
            var existingRolePermission = await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName);

            if (existingRolePermission == null) return false;

            var usersWithRoles = (await this.usersToRolesRepository.GetRolesAsync())
                .Where(x => x.RoleName == roleName)
                .ToList();

            var deleted = false;
            if (usersWithRoles.Any())
            {
                if (!removeFromUsers) return false;

                deleted = await this.usersToRolesRepository.DeleteManyByIdAsync(usersWithRoles.Select(y => y.Id));
            }

            deleted |= await this.usersToRolesRepository.DeleteByRoleNameAsync(roleName);

            return deleted;
        }
    }
}
