using System.Linq;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.Extensions
{
    public static class IdentityResultExt
    {

        public static ErrorResponse GetErrorResponse(this IdentityResult result)
        {
            IdentityErrorDescriber errorDescriber;
            var error = result.Errors.FirstOrDefault();

            return error?.Code switch
            {
                nameof(errorDescriber.DuplicateEmail) => new ErrorResponse("Email already exists."),
                nameof(errorDescriber.DuplicateUserName) => new ErrorResponse("UserName already exists."),
                _ => new ErrorResponse(error.Code),
            };
        }
    }
}
