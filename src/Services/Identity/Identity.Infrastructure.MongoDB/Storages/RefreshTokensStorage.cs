using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.Tokens.Interfaces;
using Identity.Tokens.Tokens;

namespace Identity.Infrastructure.MongoDB.Storages
{
    public class RefreshTokensStorage : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> refreshTokens;

        public RefreshTokensStorage(IMongoStorage mongoStorage)
        {
            this.refreshTokens = mongoStorage.RefreshTokens;
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            await this.refreshTokens.InsertOneAsync(refreshToken);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await this.refreshTokens.DeleteOneAsync(
                Builders<RefreshToken>.Filter.Eq(x => x.Id, id));

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAllForUserAsync(Guid userId)
        {
            var result = await this.refreshTokens.DeleteManyAsync(
                Builders<RefreshToken>.Filter.Eq(x => x.UserId, userId));

            return result.IsAcknowledged;
        }

        public async Task<RefreshToken> GetByTokenValueAsync(string tokenValue)
        {
            var refreshToken = await this.refreshTokens
                .Find(Builders<RefreshToken>.Filter.Eq(x => x.TokenValue, tokenValue))
                .SingleOrDefaultAsync();

            return refreshToken;
        }
    }
}
