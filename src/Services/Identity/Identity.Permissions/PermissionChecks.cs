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

        public static bool IsPermissionAllowed(this Claim permissionsClaim, string permissionNames)
        {
            var splitPermissions = permissionNames.Split(PermissionConstants.PermissionsSeparator);

            var isPermissionAllowed = false;
            foreach (var permissionAsChar in splitPermissions)
            {
                var parsedChar = (char)Convert.ChangeType(Enum.Parse<Permissions>(permissionAsChar), typeof(char));

                isPermissionAllowed |= permissionsClaim.Value.IsThisPermissionAllowed(parsedChar);
            }

            return isPermissionAllowed;
        }

        public static List<string> ConvertPackedPermissionsToNames(this string packedPermissions)
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

        public static string GetPackedPermissionsFromEnumerable(this IEnumerable<string> permissionNames)
        {
           var packedPermissions = "";
            foreach (var permissionName in permissionNames)
            {
                if (Enum.TryParse<Permissions>(permissionName, ignoreCase: true, out var value))
                {
                    packedPermissions += (char)Convert.ChangeType(value, typeof(char));
                }
            }

            return packedPermissions;
        }

        private static bool IsThisPermissionAllowed(this string packedPermissions, char permissionAsChar)
        {
            return packedPermissions.Contains(permissionAsChar)
                   || packedPermissions.Contains((char)ushort.MaxValue);
        }
    }
}
