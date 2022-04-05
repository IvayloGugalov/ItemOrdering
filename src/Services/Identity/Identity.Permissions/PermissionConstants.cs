namespace Identity.Permissions
{
    public static class PermissionConstants
    {
        /// <summary>
        /// Claim type for the user identifier
        /// </summary>
        public const string UserIdClaimType = "_id";

        /// <summary>
        /// Claim name that holds the packed permission string
        /// </summary>
        public const string PackedPermissionClaimType = "Permissions";

        /// <summary>
        /// This is the char for the AccessAll permission
        /// </summary>
        public const char PackedAccessAllPermission = (char)ushort.MaxValue;

        /// <summary>
        /// Character separating permissions
        /// </summary>
        public const char PermissionsSeparator = ',';
    }
}
