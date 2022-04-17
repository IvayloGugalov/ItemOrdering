using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Permissions;
using Identity.Shared;

namespace Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly IAuthenticator authenticator;
        private readonly UserManager<AuthUser> userManager;
        private readonly SignInManager<AuthUser> signInManager;

        public Login(
            IAuthenticator authenticator,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager)
        {
            this.authenticator = authenticator;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost(LoginRequest.ROUTE)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            var user = await this.userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null) return Unauthorized(new ErrorResponse("Email does not exist."));

            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPasswordCorrect) return Unauthorized(new ErrorResponse("Password is incorrect."));

            var (accessToken, refreshToken) = await this.authenticator.AuthenticateUserAsync(user);
            var roles = string.Concat(user.UserRoles.Select(x => x.Role.PackedPermissionsInRole));

            await this.signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
            var response = new UserAuthenticatedDto(accessToken, roles);

            this.HttpContext.Response.Cookies.AppendRefreshToken(refreshToken);

            return Ok(response);
        }

        // TODO: Remove
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermissions(Permissions.Permissions.Customer)]
        [HttpGet("api/login/secret")]
        public string Secret()
        {
            return "customer secret";
        }
    }
}
