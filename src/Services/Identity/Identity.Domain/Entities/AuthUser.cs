using System;
using System.Collections.Generic;
using System.Linq;

using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;

using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entities
{
    public sealed class AuthUser : MongoUser<Guid>
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

        [BsonRequired]
        [PersonalData]
        public HashSet<UserToRole> UserRoles { get; private set; }

        public AuthUser(string firstName, string lastName, string email, string username, string passwordHash, Address address, IEnumerable<RoleToPermissions> roles)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            this.LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            this.Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
            this.UserName = Guard.Against.NullOrWhiteSpace(username, nameof(username));
            this.PasswordHash = Guard.Against.NullOrWhiteSpace(passwordHash, nameof(passwordHash));
            this.Address = Guard.Against.Null(address, nameof(address));

            Guard.Against.NullOrEmpty(roles, nameof(roles));
            this.UserRoles = new HashSet<UserToRole>(roles.Select(x => new UserToRole(this.Id, x)));
        }

        public bool AddRoleToUser(RoleToPermissions role)
        {
            Guard.Against.Null(role, nameof(role));

            if (this.UserRoles.Any(x => x.RoleName == role.RoleName)) return false;

            this.UserRoles.Add(new UserToRole(this.Id, role));
            return true;
        }

        public bool RemoveRoleFromUser(RoleToPermissions role)
        {
            Guard.Against.Null(role, nameof(role));

            var foundUserToRole = this.UserRoles.SingleOrDefault(x => x.RoleName == role.RoleName);
            return this.UserRoles.Remove(foundUserToRole);
        }

        public void ReplaceAllRoles(IEnumerable<RoleToPermissions> roles)
        {
            this.UserRoles = new HashSet<UserToRole>(roles.Select(x => new UserToRole(this.Id, x)));
        }
    }
}
