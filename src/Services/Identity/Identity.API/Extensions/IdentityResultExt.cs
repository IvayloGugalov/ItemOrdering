using System.Linq;

using Microsoft.AspNetCore.Identity;

using Identity.Shared;

namespace Identity.API.Extensions
{
    // TODO: Move to Shared?
    public static class IdentityResultExt
    {
        public static ErrorResponse GetErrorResponse(this IdentityResult result)
        {
            IdentityErrorDescriber errorDescriber;
            var error = result.Errors.FirstOrDefault();

            return error?.Code switch
            {
                nameof(errorDescriber.DuplicateEmail) => new ErrorResponse(ErrorMessages.EMAIL_EXISTS),
                nameof(errorDescriber.DuplicateUserName) => new ErrorResponse(ErrorMessages.USERNAME_EXISTS),
                _ => new ErrorResponse(error.Code),
            };
        }
    }
}
