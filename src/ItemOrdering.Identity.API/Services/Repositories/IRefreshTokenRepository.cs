using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using ItemOrdering.Identity.API.Models;

namespace ItemOrdering.Identity.API.Services.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken refreshToken);
        Task<DeleteResult> DeleteAsync(Guid id);
        Task<DeleteResult> DeleteAllForUserAsync(Guid userId);
        Task<RefreshToken> GetByTokenValue(string tokenValue);
    }
}