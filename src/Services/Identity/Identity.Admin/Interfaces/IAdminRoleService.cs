using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GenericStatus;

using Identity.Domain.Entities;

namespace Identity.Admin.Interfaces
{
    public interface IAdminRoleService
    {
        IQueryable<RoleToPermissions> QueryRoleToPermissions();
        Task<bool> IsRoleNameExistingAsync(string roleName);
        IQueryable<AuthUser> QueryUsersUsingThisRole(string roleName);
        Task<IGenericStatus> CreateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null);
        Task<IGenericStatus> UpdateRoleToPermissionsAsync(string roleName, IEnumerable<string> permissionNames, string description = null);
        Task<IGenericStatus<bool>> DeleteRoleAsync(string roleName, bool removeFromUsers);
    }
}
