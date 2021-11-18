using GuardClauses;
using MongoDB.Driver;

using Identity.Domain.Entities;
using Identity.Tokens.Tokens;

namespace Identity.Infrastructure.MongoDB.Storages
{
    public class MongoStorage : IMongoStorage
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public IMongoCollection<RefreshToken> RefreshTokens { get; }
        public IMongoCollection<UserToRole> UsersToRoles { get; }
        public IMongoCollection<RoleToPermissions> RolesToPermissions { get; }

        public MongoStorage(IMongoDatabaseSettings options)
        {
            Guard.Against.NullOrEmpty(options.ConnectionString, nameof(options.ConnectionString));

            var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);

            // TODO:
            //if (string.IsNullOrEmpty(options.Username) == false)
            //{
            //    settings.Credential = MongoCredential.CreateCredential("admin", options.Username, options.Password);
            //}

            this.Client = new MongoClient(settings);

            this.Database = this.Client.GetDatabase(options.DatabaseName);

            this.RefreshTokens = this.Database.GetCollection<RefreshToken>(options.RefreshTokensCollectionName ?? "RefreshTokens");
            this.UsersToRoles = this.Database.GetCollection<UserToRole>(options.UsersToRolesCollectionName ?? "UsersToRoles");
            this.RolesToPermissions = this.Database.GetCollection<RoleToPermissions>(options.RolesToPermissionsCollectionName ?? "RolesToPermissions");
        }
    }
}
