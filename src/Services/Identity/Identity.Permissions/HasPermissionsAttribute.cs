using System;

using Microsoft.AspNetCore.Authorization;

namespace Identity.Permissions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// This creates a HasPermissions attribute with the Permissions enum member
        /// </summary>
        /// <param name="permissions"></param>
        public HasPermissionsAttribute(params Permissions[] permissions) : base(string.Join(PermissionConstants.PermissionsSeparator, permissions))
        {
            if (permissions == null || permissions.Length < 1) throw new ArgumentNullException(nameof(permissions));
        }
    }
}
