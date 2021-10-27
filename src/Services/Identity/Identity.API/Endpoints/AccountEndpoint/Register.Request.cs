using System.ComponentModel.DataAnnotations;

using Identity.Domain.Entities;

namespace Identity.API.Endpoints.AccountEndpoint
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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password does not match confirm password.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public Address Address { get; set; }
    }
}
