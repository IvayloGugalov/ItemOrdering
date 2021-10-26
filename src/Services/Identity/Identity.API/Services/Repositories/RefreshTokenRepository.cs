using System;
using System.Threading.Tasks;

using MongoDB.Driver;

using Identity.API.Extensions;
using Identity.API.Models;

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

        public async Task<DeleteResult> DeleteAsync(Guid id)
        {
            var result = await this.refreshTokensCollection.DeleteOneAsync(rt => rt.Id == id);

            return result;
        }

        public async Task<DeleteResult> DeleteAllForUserAsync(Guid userId)
        {
            var result = await this.refreshTokensCollection.DeleteManyAsync(rt => rt.UserId == userId);

            return result;
        }

        public async Task<RefreshToken> GetByTokenValue(string tokenValue)
        {
            var refreshToken = await this.refreshTokensCollection.Find(rt => rt.TokenValue == tokenValue)
                .SingleOrDefaultAsync();

            return refreshToken;
        }
    }
}
