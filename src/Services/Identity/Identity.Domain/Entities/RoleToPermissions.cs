using System;
using System.Linq;

using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;

using Identity.Domain.Exceptions;
using Identity.Permissions;

namespace Identity.Domain.Entities
{
    public class RoleToPermissions
    {
        [BsonId]
        public string RoleName { get; private set; }

        /// <summary>
        /// Name of the permission presentable to the UI
        /// </summary>
        [BsonRequired]
        public string DisplayName { get; private set; }

        /// This contains the list of permissions as a series of unicode chars
        [BsonRequired]
        public string PackedPermissionsInRole { get; private set; }

        [BsonRequired]
        public string Description { get; private set; }

        /// <summary>
        /// Used when we create a new role that doesn't exist in the database
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="packedPermissions"></param>
        public RoleToPermissions(string roleName, string description, string packedPermissions)
        {
            this.RoleName = Guard.Against.NullOrWhiteSpace(roleName, nameof(roleName))
                .Where(c => !char.IsWhiteSpace(c)).ToString();
            this.DisplayName = roleName;
            this.Update(packedPermissions, description);
        }

        public RoleToPermissions(Permissions.Permissions permissions)
        {
            var (roleName, description) = permissions.GetNameAndDescription();
            this.RoleName = Guard.Against.NullOrWhiteSpace(roleName, nameof(roleName));
            this.DisplayName = Guard.Against.NullOrWhiteSpace(permissions.GetDisplayName(), nameof(this.DisplayName));

            this.Update(permissions.GetPermissionAsChar().ToString(), description);
        }

        public void Update(string packedPermissions, string description = null)
        {
            // TODO: Update Guard Clause to throw custom exceptions
            this.PackedPermissionsInRole = !string.IsNullOrEmpty(packedPermissions)
                ? packedPermissions
                : throw new AuthPermissionsException("There should be at least one permission per role.");
            this.Description = description?.Trim() ?? this.Description;
        }

        public override string ToString()
        {
            var description = this.Description == null ? "" : $"(description = {this.Description})";
            return $"{this.RoleName} {description} has {this.PackedPermissionsInRole.Length} permissions.";
        }
    }
}
