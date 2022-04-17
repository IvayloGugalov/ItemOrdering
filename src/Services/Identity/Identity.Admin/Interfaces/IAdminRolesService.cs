using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GenericStatus;

using Identity.Domain.Entities;

namespace Identity.Admin.Interfaces
{
    public interface IAdminRolesService
    {
        IQueryable<RoleToPermissions> QueryRoleToPermissions();
        Task<bool> IsRoleNameExistingAsync(string roleName);
        IQueryable<AuthUser> QueryUsersByRole(string roleName);
        Task<IGenericStatus<RoleToPermissions>> CreateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null);
        Task<IGenericStatus<RoleToPermissions>> UpdateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null);
        Task<IGenericStatus<bool>> DeleteRoleAsync(string roleName, bool removeFromUsers);
    }
}
