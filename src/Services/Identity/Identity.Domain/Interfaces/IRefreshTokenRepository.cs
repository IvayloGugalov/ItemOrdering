using System;
using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken refreshToken);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllForUserAsync(Guid userId);
        Task<RefreshToken> GetByTokenValueAsync(string tokenValue);
    }
}
