using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly IAuthenticator authenticator;
        private readonly UserManager<AuthUser> userManager;

        public Login(IAuthenticator authenticator, UserManager<AuthUser> userManager)
        {
            this.authenticator = authenticator;
            this.userManager = userManager;
        }

        [HttpPost(LoginRequest.ROUTE)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> LoginWithManagerAsync([FromBody]LoginRequest loginRequest)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var user = await this.userManager.FindByNameAsync(loginRequest.Username);
            if (user is null) return Unauthorized(new ErrorResponse("UserName does not exist."));

            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPasswordCorrect) return Unauthorized(new ErrorResponse("Password is incorrect."));

            var response = await this.authenticator.AuthenticateUserAsync(user);

            return Ok(response);
        }
    }
}
