using System.Linq;
using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IRoleToPermissionRepository
    {
        IQueryable<RoleToPermissions> QueryRoleToPermissions();
        Task<RoleToPermissions> GetByRoleNameAsync(string roleName);
        Task CreateAsync(RoleToPermissions roleToPermissions);
        Task UpdateAsync(RoleToPermissions existingRolePermission);
    }
}
