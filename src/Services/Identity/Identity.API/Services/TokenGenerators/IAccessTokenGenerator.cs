using Identity.API.Models;

namespace Identity.API.Services.TokenGenerators
{
    public interface IAccessTokenGenerator
    {
        string GenerateAccessToken(User user);
    }
}