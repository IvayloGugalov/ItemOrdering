using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Infrastructure.MongoDB.Storages
{
    public class UsersToRolesStorage : IUserToRoleRepository
    {
        private readonly IMongoCollection<UserToRole> usersToRoles;

        public UsersToRolesStorage(IMongoStorage mongoStorage)
        {
            this.usersToRoles = mongoStorage.UsersToRoles;
        }

        public async Task<IEnumerable<UserToRole>> GetRolesForAuthUserAsync(Guid userId)
        {
            var rolesForUser = await this.usersToRoles.FindAsync(
                Builders<UserToRole>.Filter.Eq(x => x.UserId, userId));

            return await rolesForUser.ToListAsync();
        }

        public async Task<IEnumerable<UserToRole>> GetRolesAsync()
        {
            return await this.usersToRoles.AsQueryable().ToListAsync();
        }

        public async Task CreateAsync(UserToRole userToRole)
        {
            await this.usersToRoles.InsertOneAsync(userToRole);
        }

        public async Task<bool> DeleteByUserIdAsync(Guid userId)
        {
            var result = await this.usersToRoles.DeleteOneAsync(
                Builders<UserToRole>.Filter.Eq(x => x.UserId, userId));

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteManyByIdAsync(IEnumerable<ObjectId> userToRoleIds)
        {
            var result = await this.usersToRoles.DeleteManyAsync(
                Builders<UserToRole>.Filter.In(x => x.Id, userToRoleIds));

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteByRoleNameAsync(string roleName)
        {
            var result = await this.usersToRoles.DeleteOneAsync(
                Builders<UserToRole>.Filter.Eq(x => x.RoleName, roleName));

            return result.IsAcknowledged;
        }
    }
}
