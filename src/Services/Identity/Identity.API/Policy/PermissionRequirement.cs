using GuardClauses;

using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Policy
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            this.PermissionName = Guard.Against.NullOrEmpty(permissionName, nameof(permissionName));
        }
    }
}
