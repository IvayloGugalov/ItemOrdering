namespace Identity.API.Endpoints.AdminEndpoint
{
    public record RoleToPermissionDto(string RoleName, string PackedPermissions);
}