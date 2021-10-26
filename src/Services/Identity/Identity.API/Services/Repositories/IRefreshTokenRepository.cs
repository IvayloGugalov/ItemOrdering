using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.API.Models;

namespace Identity.API.Services.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken refreshToken);
        Task<DeleteResult> DeleteAsync(Guid id);
        Task<DeleteResult> DeleteAllForUserAsync(Guid userId);
        Task<RefreshToken> GetByTokenValue(string tokenValue);
    }
}