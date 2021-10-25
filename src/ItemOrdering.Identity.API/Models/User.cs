using System;

using AspNetCore.Identity.Mongo.Model;
using GuardClauses;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace ItemOrdering.Identity.API.Models
{
    public sealed class User : MongoUser<Guid>
    {
        [BsonRequired]
        [PersonalData]
        public string FirstName { get; private set; }

        [BsonRequired]
        [PersonalData]
        public string LastName { get; private  set; }

        [BsonRequired]
        [PersonalData]
        public Address Address { get; private set; }

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
