using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Identity.Domain;

namespace Identity.API.Policy
{
    public static class PermissionChecks
    {
        public static bool HasPermission<TEnumPermission>(this ClaimsPrincipal user, TEnumPermission permissionToCheck)
            where TEnumPermission : Enum
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

        public static Guid GetUserIdFromClaims(this IEnumerable<Claim> claims)
        {
            var unparsedId = claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(unparsedId, out var userId)
                ? userId
                : Guid.Empty;
        }

        public static List<string> PermissionsFromUser(ClaimsPrincipal user)
        {
            var packedPermissions = user.GetPackedPermissionsFromUser();

            return packedPermissions.ConvertPackedPermissionToNames(typeof(Permissions));
        }

        public static bool ThisPermissionIsAllowed(this Type enumPermissionType, string packedPermissions, string permissionName)
        {
            var permissionAsChar = (char)Convert.ChangeType(Enum.Parse(enumPermissionType, permissionName), typeof(char));
            return packedPermissions.IsThisPermissionAllowed(permissionAsChar);
        }

        public static List<string> ConvertPackedPermissionToNames(this string packedPermissions, Type permissionsEnumType)
        {
            if (packedPermissions == null) return null;

            var permissionNames = new List<string>();
            foreach (var permissionChar in packedPermissions)
            {
                var enumName = Enum.GetName(permissionsEnumType, (ushort)permissionChar);
                if (enumName == null) continue;

                permissionNames.Add(enumName);
            }

            return permissionNames;
        }

        private static bool IsThisPermissionAllowed(this string packedPermissions, char permissionAsChar)
        {
            return packedPermissions.Contains(permissionAsChar) ||
                   packedPermissions.Contains((char)ushort.MaxValue);
        }
    }
}
