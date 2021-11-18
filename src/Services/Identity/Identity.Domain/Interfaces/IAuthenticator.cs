using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Tokens;

namespace Identity.Domain.Interfaces
{
    public interface IAuthenticator
    {
        Task<AccessAndRefreshToken> AuthenticateUserAsync(AuthUser user);
    }
}