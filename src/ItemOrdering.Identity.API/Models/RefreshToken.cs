using System;

using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemOrdering.Identity.API.Models
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRequired]
        public Guid Id { get; }

        [BsonRequired]
        public string TokenValue { get; }

        [BsonRequired]
        public Guid UserId { get; }

        [BsonRequired]
        public DateTime AddedDateUtc { get; }

        public RefreshToken(string tokenValue, Guid userId)
        {
            this.Id = Guid.NewGuid();
            this.AddedDateUtc = DateTime.UtcNow;
            this.TokenValue = Guard.Against.NullOrWhiteSpace(tokenValue, nameof(tokenValue));
            this.UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
        }
    }
}
