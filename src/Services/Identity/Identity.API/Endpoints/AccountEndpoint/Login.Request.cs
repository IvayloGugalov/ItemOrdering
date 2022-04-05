using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AccountEndpoint
{
    public class LoginRequest
    {
        public const string ROUTE = "api/login";

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}