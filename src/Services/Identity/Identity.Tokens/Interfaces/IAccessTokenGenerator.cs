using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Tokens.Interfaces
{
    public interface IAccessTokenGenerator
    {
        Task<string> GenerateAccessToken(Func<Task<List<Claim>>> getClaimsFunc, Guid userId, string userEmail, string userName);
    }
}
