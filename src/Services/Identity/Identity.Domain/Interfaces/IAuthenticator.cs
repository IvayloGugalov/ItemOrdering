using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IAuthenticator
    {
        Task<AuthenticationTokens> AuthenticateUserAsync(AuthUser user);
    }
}