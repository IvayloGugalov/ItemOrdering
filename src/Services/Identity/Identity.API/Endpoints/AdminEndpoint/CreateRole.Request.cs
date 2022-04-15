using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public class CreateRoleRequest
    {
        public const string ROUTE = "api/admin/create-role";

        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> PermissionNames { get; set; }
    }
}
