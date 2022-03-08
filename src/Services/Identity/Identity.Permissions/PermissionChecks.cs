using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Identity.Permissions
{
    public static class PermissionChecks
    {
        public static bool HasPermission(this ClaimsPrincipal user, Permissions permissionToCheck)
        {
            var packedPermissions = user.GetPackedPermissionsFromUser();
            if (packedPermissions == null) return false;

            var permissionAsChar = (char)Convert.ChangeType(permissionToCheck, typeof(char));
            return packedPermissions.IsThisPermissionAllowed(permissionAsChar);
        }

        public static string GetPackedPermissionsFromUser(this ClaimsPrincipal user)
        {
            return user?.Claims.SingleOrDefault(x => x.Type == PermissionConstants.PackedPermissionClaimType)?.Value;
        }

        public static bool IsPermissionAllowed(this Claim permissionsClaim, string permissionName)
        {
            var permissionAsChar = (char)Convert.ChangeType(Enum.Parse(typeof(Permissions), permissionName), typeof(char));
            return permissionsClaim.Value.IsThisPermissionAllowed(permissionAsChar);
        }

        public static List<string> ConvertPackedPermissionToNames(this string packedPermissions)
        {
            if (packedPermissions == null) return null;

            var permissionNames = new List<string>();
            foreach (var permissionChar in packedPermissions)
            {
                var enumName = Enum.GetName(typeof(Permissions), (ushort)permissionChar);
                if (enumName == null) continue;

                permissionNames.Add(enumName);
            }

            return permissionNames;
        }

        private static bool IsThisPermissionAllowed(this string packedPermissions, char permissionAsChar)
        {
            return packedPermissions.Contains(permissionAsChar)
                   || packedPermissions.Contains((char)ushort.MaxValue);
        }
    }
}
