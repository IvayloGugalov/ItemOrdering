using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    public class LoginRequest
    {
        public const string ROUTE = "api/login";

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
