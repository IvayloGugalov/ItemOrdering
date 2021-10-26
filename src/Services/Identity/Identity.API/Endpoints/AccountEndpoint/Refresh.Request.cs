using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AccountEndpoint
{
    public class RefreshRequest
    {
        public const string ROUTE = "api/refresh";

        [Required]
        public string RefreshTokenValue { get; set; }
    }
}
