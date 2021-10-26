using System.Threading.Tasks;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.API.Models;

namespace Identity.API.Services.Authenticators
{
    public interface IAuthenticator
    {
        Task<LoginResponse> AuthenticateUserAsync(User user);
    }
}