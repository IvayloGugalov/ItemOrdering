using System;

using Microsoft.AspNetCore.Authorization;

namespace Identity.Shared
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// This creates a HasPermission attribute with a Permission enum member
        /// </summary>
        /// <param name="permission"></param>
        public HasPermissionAttribute(object permission) : base(permission?.ToString()!)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var type = permission.GetType();
        }
    }
}
