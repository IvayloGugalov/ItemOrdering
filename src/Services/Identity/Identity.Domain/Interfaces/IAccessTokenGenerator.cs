using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IAccessTokenGenerator
    {
        string GenerateAccessToken(AuthUser user);
    }
}