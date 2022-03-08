using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;

using Identity.Domain.Exceptions;

namespace Identity.Domain.Entities
{
    public class RoleToPermissions
    {
        [BsonId]
        public string RoleName { get; private set; }

        /// This contains the list of permissions as a series of unicode chars
        [BsonRequired]
        public string PackedPermissionsInRole { get; private set; }

        [BsonRequired]
        public string Description { get; private set; }

        public RoleToPermissions(string roleName, string description, string packedPermissions)
        {
            this.RoleName = Guard.Against.NullOrWhiteSpace(roleName, nameof(roleName));
            this.Update(packedPermissions, description);
        }

        public void Update(string packedPermissions, string description = null)
        {
            if (string.IsNullOrEmpty(packedPermissions)) throw new AuthPermissionsException("There should be at least one permission per role.");

            this.PackedPermissionsInRole = packedPermissions;
            this.Description = description?.Trim() ?? this.Description;
        }

        public override string ToString()
        {
            var description = this.Description == null ? "" : $"(description = {this.Description})";
            return $"{this.RoleName} {description} has {this.PackedPermissionsInRole.Length} permissions.";
        }
    }
}
