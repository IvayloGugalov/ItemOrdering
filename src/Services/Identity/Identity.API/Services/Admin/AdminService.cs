using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Extensions;
using Identity.Domain;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Identity.API.Services.Admin
{
    class AdminService
    {
        private readonly AuthPermissionsOptions options;
        private readonly IMongoCollection<AuthUser> usersCollection;
        private readonly UserManager<AuthUser> userManager;

        //public AdminService(IMongoDatabaseSettings settings, AuthPermissionsOptions options)
        //{
        //    this.usersCollection = MongoExtension.GetCollection<AuthUser>(settings, settings.UsersCollectionName);
        //    this.options = options;
        //}

        public async Task<IEnumerable<AuthUser>> QueryAuthUsersAsync()
        {
            return await this.usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<AuthUser> FindAuthUserByIdAsync(Guid userId)
        {
            var user = await this.usersCollection.Find(rt => rt.Id == userId)
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task<AuthUser> FindAuthUserByEmailAsync(string email)
        {
            var user = await this.usersCollection.Find(rt => rt.Email == email)
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task<bool> DeleteAuthUserAsync(Guid userId)
        {
            var user = await this.FindAuthUserByIdAsync(userId);

            if (user == null) return false;

            var result = await this.usersCollection.DeleteOneAsync(rt => rt.Id == userId);

            return result.IsAcknowledged;
        }

        // TODO
        public async Task<AuthUser> UpdateUserAsync(Guid userId, string email, string userName, List<string> roleNames)
        {
            var result = await this.FindAuthUserByIdAsync(userId);

            if (result == null) return null;

            return null;
        }

    }
}
