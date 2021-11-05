using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.Domain;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

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

            var user = await this.userManager.FindByNameAsync(loginRequest.Username);
            if (user is null) return Unauthorized(new ErrorResponse("UserName does not exist."));

            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPasswordCorrect) return Unauthorized(new ErrorResponse("Password is incorrect."));

            var (newAccessToken, newRefreshToken) = await this.authenticator.AuthenticateUserAsync(user);
            var response = new UserAuthenticatedDto(newAccessToken, newRefreshToken);

            await this.signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

            return Ok(response);
        }

        // TODO: Remove
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HasPermission(Permissions.Customer)]
        [HttpGet("api/login/secret")]
        public string Secret()
        {
            return "customer secret";
        }
    }

    // TODO: Remove, use it inside the projects calling the Auth Api
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// This creates an HasPermission attribute with a Permission enum member
        /// </summary>
        /// <param name="permission"></param>
        public HasPermissionAttribute(object permission) : base(permission?.ToString()!)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var type = permission.GetType();
        }
    }
}
