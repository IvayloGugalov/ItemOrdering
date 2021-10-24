using System.Threading.Tasks;

using ItemOrdering.Identity.API.Endpoints.AccountEndpoint;
using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Services.Authenticators
{
    public interface IAuthenticator
    {
        Task<LoginResponse> AuthenticateUserAsync(User user);
    }
}