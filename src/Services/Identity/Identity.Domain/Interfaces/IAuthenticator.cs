using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IAuthenticator
    {
        Task<AccessAndRefreshToken> AuthenticateUserAsync(AuthUser user);
    }
}