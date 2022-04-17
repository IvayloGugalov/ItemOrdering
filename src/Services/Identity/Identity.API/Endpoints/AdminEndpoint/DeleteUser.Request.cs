using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AdminEndpoint
{
    public class DeleteUserRequest
    {
        public const string ROUTE = "api/admin/delete-user";

        [Required]
        public string Email { get; set; }
    }
}