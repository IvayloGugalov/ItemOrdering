using System.ComponentModel.DataAnnotations;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    public class LoginRequest
    {
        public const string ROUTE = "api/login";

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
