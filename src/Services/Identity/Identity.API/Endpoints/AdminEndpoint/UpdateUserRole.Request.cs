using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public class UpdateUserRoleRequest
    {
        public const string ROUTE = "api/admin/update-user-role";

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
