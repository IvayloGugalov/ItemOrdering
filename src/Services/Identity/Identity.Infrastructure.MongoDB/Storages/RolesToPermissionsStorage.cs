using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Infrastructure.MongoDB.Storages
{
    public class RolesToPermissionsStorage : IRoleToPermissionRepository
    {
        private readonly IMongoCollection<RoleToPermissions> rolesToPermissions;

        public RolesToPermissionsStorage(IMongoStorage mongoStorage)
        {
            this.rolesToPermissions = mongoStorage.RolesToPermissions;
        }

        public IQueryable<RoleToPermissions> QueryRoleToPermissions()
        {
            return this.rolesToPermissions.AsQueryable();
        }

        public async Task<RoleToPermissions> GetByRoleNameAsync(string roleName)
        {
            var rolesForRoleName = await this.rolesToPermissions.FindAsync(
                Builders<RoleToPermissions>.Filter.Eq(x => x.RoleName, roleName));

            return await rolesForRoleName.SingleOrDefaultAsync();
        }

        public async Task CreateAsync(RoleToPermissions roleToPermissions)
        {
            await this.rolesToPermissions.InsertOneAsync(roleToPermissions);
        }

        public async Task UpdateAsync(RoleToPermissions existingRolePermission)
        {
            await this.rolesToPermissions.FindOneAndUpdateAsync(
                Builders<RoleToPermissions>.Filter
                    .Eq(x => x.RoleName, existingRolePermission.RoleName),
                Builders<RoleToPermissions>.Update
                    .Set(x => x.PackedPermissionsInRole, existingRolePermission.PackedPermissionsInRole));
        }
    }
}
