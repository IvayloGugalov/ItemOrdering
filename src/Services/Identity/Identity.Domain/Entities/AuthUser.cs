using System;
using System.Collections.Generic;
using System.Linq;

using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;

using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;
using GuidGenerator;

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

        public IReadOnlyCollection<UserToRole> UserRoles => this.userRoles?.ToList();
        [BsonRequired]
        [PersonalData]
        private HashSet<UserToRole> userRoles;

        public AuthUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<RoleToPermissions> roles,
            IGuidGeneratorService guidGenerator)
        {
            this.Id = guidGenerator.GenerateGuid();
            this.FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            this.LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            this.Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
            this.UserName = Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
            this.PasswordHash = Guard.Against.NullOrWhiteSpace(password, nameof(password));

            Guard.Against.NullOrEmpty(roles, nameof(roles));
            this.userRoles = new HashSet<UserToRole>(roles.Select(x => new UserToRole(this.Id, x)));
        }

        public bool AddRoleToUser(RoleToPermissions role)
        {
            Guard.Against.Null(role, nameof(role));

            if (this.UserRoles.Any(x => x.RoleName == role.RoleName)) return false;

            this.userRoles.Add(new UserToRole(this.Id, role));
            return true;
        }

        public bool RemoveRoleFromUser(RoleToPermissions role)
        {
            Guard.Against.Null(role, nameof(role));

            var foundUserToRole = this.UserRoles.SingleOrDefault(x => x.RoleName == role.RoleName);
            return this.userRoles.Remove(foundUserToRole);
        }

        public void ReplaceAllRoles(IEnumerable<RoleToPermissions> roles)
        {
            this.userRoles = new HashSet<UserToRole>(roles.Select(x => new UserToRole(this.Id, x)));
        }

        public void UpdateUserNameAndEmail(string userName, string email)
        {
            this.UserName = Guard.Against.NullOrWhiteSpace(userName, nameof(userName));
            this.Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
        }
    }
}
