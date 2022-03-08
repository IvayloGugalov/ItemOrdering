using System.ComponentModel.DataAnnotations;

namespace Identity.API.Endpoints.AccountEndpoint
{
    public class PasswordResetRequest
    {
        public const string ROUTE = "api/account/resetpassword";

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New password does not match confirm password.")]
        public string ConfirmNewPassword { get; set; }
    }
}
