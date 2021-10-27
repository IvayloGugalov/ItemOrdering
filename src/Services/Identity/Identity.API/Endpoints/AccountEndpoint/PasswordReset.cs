using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.Domain.Entities;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class PasswordReset : ControllerBase
    {
        private readonly UserManager<AuthUser> userManager;

        public PasswordReset(UserManager<AuthUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost(PasswordResetRequest.ROUTE)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]PasswordResetRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user == null) return NotFound(new ErrorResponse("User with this email not found"));

            var passwordResetToken = this.userManager.GeneratePasswordResetTokenAsync(user);

            var result = await this.userManager.ResetPasswordAsync(user, passwordResetToken.Result, request.NewPassword);
            if (!result.Succeeded) return BadRequest(new ErrorResponse("Unable to reset password"));

            return Ok();
        }
    }
}
