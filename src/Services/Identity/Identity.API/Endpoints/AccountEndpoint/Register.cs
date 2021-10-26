using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Identity.API.Models;

namespace Identity.API.Endpoints.AccountEndpoint
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

               return error?.Code switch
               {
                   nameof(errorDescriber.DuplicateEmail) => Conflict(new ErrorResponse("Email already exists.")),
                   nameof(errorDescriber.DuplicateUserName) => Conflict(new ErrorResponse("UserName already exists.")),
                   _ => Conflict(new ErrorResponse(error.Code)),
               };
           }

            return Ok(result);
        }
    }
}
