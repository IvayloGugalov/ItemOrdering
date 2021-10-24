using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Identity.API.Extensions;
using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Endpoints.AccountEndpoint
{
    [ApiController]
    public class Register : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public Register(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost(RegisterRequest.ROUTE)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterWithManagerAsync([FromBody] RegisterRequest request)
        {
            if (!this.ModelState.IsValid) return BadRequest(GetModelErrorMessages.BadRequestModelState(this.ModelState));

            if (request.Password != request.ConfirmPassword) return BadRequest(new ErrorResponse("Password does not match confirm password."));

            var registrationUser = new User(
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email,
                username: request.Username,
                passwordHash: request.Password,
                address: request.Address);

           var result = await this.userManager.CreateAsync(registrationUser, request.Password);

           if (!result.Succeeded)
           {
                IdentityErrorDescriber errorDescriber;
                var error = result.Errors.FirstOrDefault();

                switch (error?.Code)
                {
                    case nameof(errorDescriber.DuplicateEmail):
                        return Conflict(new ErrorResponse("Email already exists."));
                    case nameof(errorDescriber.DuplicateUserName):
                        return Conflict(new ErrorResponse("UserName already exists."));
                    default:
                        return Conflict(new ErrorResponse(error.Code));
                }
           }

            return Ok(result);
        }
    }
}
