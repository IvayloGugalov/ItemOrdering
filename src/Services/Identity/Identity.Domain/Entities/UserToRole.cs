using System;

using AspNetCore.Identity.Mongo.Model;
using GuardClauses;

namespace Identity.Domain.Entities
{
    public class UserToRole : MongoRole<Guid>
    {
        public Guid UserId { get; private set; }
        public RoleToPermissions Role { get; private set; }
        public string RoleName { get; private set; }

        public UserToRole(Guid userId, RoleToPermissions role)
        {
            this.UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
            this.Role = Guard.Against.Null(role, nameof(role));
            this.RoleName = role.RoleName;
        }
    }
}
