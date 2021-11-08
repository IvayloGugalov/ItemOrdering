using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.API.Services.Repositories
{
    public class UserToRoleRepository : IUserToRoleRepository
    {
        private readonly IMongoCollection<UserToRole> usersToRolesCollection;

        public UserToRoleRepository(IMongoDatabaseSettings settings)
        {
            this.usersToRolesCollection = MongoExtension.GetCollection<UserToRole>(settings, settings.UsersToRolesCollectionName);
        }

        public async Task<IEnumerable<UserToRole>> GetRolesForAuthUserAsync(Guid userId)
        {
            return await this.usersToRolesCollection.Find(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserToRole>> GetRolesAsync()
        {
            return await this.usersToRolesCollection.AsQueryable().ToListAsync();
        }

        public async Task CreateAsync(UserToRole userToRole)
        {
            await this.usersToRolesCollection.InsertOneAsync(userToRole);
        }

        public async Task<bool> DeleteByUserIdAsync(Guid userId)
        {
            var result = await this.usersToRolesCollection.DeleteOneAsync(rt => rt.UserId == userId);

            return result.IsAcknowledged;
        }
    }
}
