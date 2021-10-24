using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Services.TokenGenerators
{
    public interface IAccessTokenGenerator
    {
        string GenerateAccessToken(User user);
    }
}