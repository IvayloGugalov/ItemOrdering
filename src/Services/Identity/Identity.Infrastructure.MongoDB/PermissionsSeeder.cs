using System;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.Domain.Entities;
using Identity.Infrastructure.MongoDB.Storages;
using Identity.Permissions;

namespace Identity.Infrastructure.MongoDB
{
    public interface IPermissionsSeeder
    {
        Task SeedDbWithPermissions();
    }

    public class PermissionsSeeder : IPermissionsSeeder
    {
        private readonly IMongoStorage mongoStorage;

        public PermissionsSeeder(IMongoStorage mongoStorage)
        {
            this.mongoStorage = mongoStorage;
        }

        public async Task SeedDbWithPermissions()
        {
            var roleToPermissionCollection = this.mongoStorage.RolesToPermissions;

            if ((await roleToPermissionCollection.AsQueryable().ToListAsync()).Any()) return;

            var permissions = (Permissions.Permissions[])Enum.GetValues(typeof(Permissions.Permissions));

            foreach (var permission in permissions)
            {
                await roleToPermissionCollection.InsertOneAsync(new RoleToPermissions(permission));
            }
        }
    }
}
