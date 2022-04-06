using GuardClauses;

using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Policy
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionNames { get; }

        public PermissionRequirement(string permissionNames)
        {
            this.PermissionNames = Guard.Against.NullOrEmpty(permissionNames, nameof(permissionNames));
        }
    }
}
