using System;

using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

using GuardClauses;

namespace ItemOrdering.Identity.API.Models
{
    public sealed class User : MongoUser<Guid>
    {
        [BsonRequired]
        public string FirstName { get; }

        [BsonRequired]
        public string LastName { get; }

        [BsonRequired]
        public Address Address { get; }

        public User(string firstName, string lastName, string email, string username, string passwordHash, Address address)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            this.LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            this.Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
            this.UserName = Guard.Against.NullOrWhiteSpace(username, nameof(username));
            this.PasswordHash = Guard.Against.NullOrWhiteSpace(passwordHash, nameof(passwordHash));
            this.Address = Guard.Against.Null(address, nameof(address));
        }
    }
}
