using System;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Tokens;

namespace Identity.Domain.Interfaces
{
    public interface IAuthenticator
    {
        Task<AccessAndRefreshToken> AuthenticateUserAsync(AuthUser user);

        Task<string> RefreshAccessToken(AuthUser user);

        Task<string> RefreshRefreshToken(Guid userId);
    }
}