using System;
using System.Threading.Tasks;

using Identity.Tokens.Tokens;

namespace Identity.Tokens.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken refreshToken);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllForUserAsync(Guid userId);
        Task<RefreshToken> GetByTokenValueAsync(string tokenValue);
    }
}
