using System;

using GuardClauses;
using GuidGenerator;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local used for deserialization with Mongo
namespace Identity.Tokens.Tokens
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRequired]
        public Guid Id { get; private set; }

        [BsonRequired]
        public string TokenValue { get; private set; }

        [BsonRequired]
        public Guid UserId { get; private set; }

        [BsonRequired]
        public DateTime AddedDateUtc { get; private set; }

        public RefreshToken(
            string tokenValue,
            Guid userId,
            IGuidGeneratorService guidGenerator)
        {
            this.Id = guidGenerator.GenerateGuid();
            this.AddedDateUtc = DateTime.UtcNow;
            this.TokenValue = Guard.Against.NullOrWhiteSpace(tokenValue, nameof(tokenValue));
            this.UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
        }
    }
}