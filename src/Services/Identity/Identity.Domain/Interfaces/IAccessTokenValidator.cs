using System.Security.Claims;

namespace Identity.Domain.Interfaces
{
    public interface IAccessTokenValidator
    {
        TokenValidationResult Validate(string accessTokenValue, out ClaimsPrincipal claimsPrincipal);
    }
}