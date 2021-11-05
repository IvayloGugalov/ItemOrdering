using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IAccessTokenGenerator
    {
        Task<string> GenerateAccessToken(AuthUser user);
    }
}