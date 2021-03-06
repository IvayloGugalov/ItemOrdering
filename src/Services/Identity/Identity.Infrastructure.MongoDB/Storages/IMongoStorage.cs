using Identity.Domain.Entities;
using Identity.Tokens.Tokens;

using MongoDB.Driver;

namespace Identity.Infrastructure.MongoDB.Storages
{
    public interface IMongoStorage
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        string DatabaseName { get; }

        IMongoCollection<AuthUser> Users { get; }
        IMongoCollection<RefreshToken> RefreshTokens { get; }
        IMongoCollection<UserToRole> UsersToRoles { get; }
        IMongoCollection<RoleToPermissions> RolesToPermissions { get; }
    }
}