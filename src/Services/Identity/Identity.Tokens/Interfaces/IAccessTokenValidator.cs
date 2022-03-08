using System.Security.Claims;

namespace Identity.Tokens.Interfaces
{
    public interface IAccessTokenValidator
    {
        TokenValidationResult Validate(string accessTokenValue, out ClaimsPrincipal claimsPrincipal);
    }
}
