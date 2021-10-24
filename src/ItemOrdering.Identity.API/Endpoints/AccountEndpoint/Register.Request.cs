using System.ComponentModel.DataAnnotations;

using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    public class RegisterRequest
    {
        public const string ROUTE = "api/register";

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
