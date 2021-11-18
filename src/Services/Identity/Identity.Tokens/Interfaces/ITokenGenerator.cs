using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Tokens.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string secretKey, string issuer, string audience, double tokenExpirationMinutes, IEnumerable<Claim> claims = null);
    }
}
