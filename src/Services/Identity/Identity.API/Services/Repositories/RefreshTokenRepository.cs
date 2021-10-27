using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.API.Services.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> refreshTokensCollection;

        public RefreshTokenRepository(IMongoDatabaseSettings settings)
        {
            this.refreshTokensCollection = MongoExtension.GetCollection<RefreshToken>(settings, settings.RefreshTokensCollectionName);
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            await this.refreshTokensCollection.InsertOneAsync(refreshToken);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await this.refreshTokensCollection.DeleteOneAsync(rt => rt.Id == id);

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAllForUserAsync(Guid userId)
        {
            var result = await this.refreshTokensCollection.DeleteManyAsync(rt => rt.UserId == userId);

            return result.IsAcknowledged;
        }

        public async Task<RefreshToken> GetByTokenValueAsync(string tokenValue)
        {
            var refreshToken = await this.refreshTokensCollection.Find(rt => rt.TokenValue == tokenValue)
                .SingleOrDefaultAsync();

            return refreshToken;
        }
    }
}
