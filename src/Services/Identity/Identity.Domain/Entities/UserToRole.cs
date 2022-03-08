using System;

using GuardClauses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace Identity.Domain.Entities
{
    public class UserToRole
    {
        [BsonId]
        public ObjectId Id { get; private set; }

        [BsonRequired]
        public Guid UserId { get; private set; }

        [BsonRequired]
        public RoleToPermissions Role { get; private set; }

        [BsonRequired]
        public string RoleName { get; private set; }

        public UserToRole(Guid userId, RoleToPermissions role)
        {
            this.Id = ObjectId.GenerateNewId();
            this.UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
            this.Role = Guard.Against.Null(role, nameof(role));
            this.RoleName = role.RoleName;
        }
    }
}
