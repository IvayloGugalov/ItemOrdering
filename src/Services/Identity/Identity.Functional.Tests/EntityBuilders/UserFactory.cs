using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using Identity.API;
using Identity.Domain.Entities;
using Identity.Functional.Tests.Proxies;

namespace Identity.Functional.Tests.EntityBuilders
{
    public class UserFactory
    {
        private readonly TestIdentityWebAppFactory<Startup> factory;

        private static readonly string TEST_DATA_JSON =
            Path.Combine(
                Path.GetFullPath(
                    Path.Combine("..", "..", "..", "..", "Identity.Functional.Tests")),
                "test_data.json");

        private readonly Random random = new();

        /// <summary>
        /// 500 randomly generated users
        /// </summary>
        public TestAuthUser[] TestUsers { get; }


        public UserFactory(TestIdentityWebAppFactory<Startup> factory)
        {
            this.factory = factory;
            this.TestUsers = LoadTestUsersFromFile().ToArray();
        }

        /// <summary>
        /// Get a random user from the test data.
        /// </summary>
        /// <returns></returns>
        public TestAuthUser GetRandomUser() => this.TestUsers[random.Next(this.TestUsers.Length - 1)];

        public AuthUser CreateAuthUser()
        {
            var randomUser = this.GetRandomUser();

            var rolesArray = randomUser.Permissions.Split(',');

            var roleToPermissionsList = rolesArray.Select(s => new RoleToPermissions(Enum.Parse<Permissions.Permissions>(s)))
                .ToList();

            using var scope = this.factory.Services.CreateScope();
            var userManagerProxy = scope.ServiceProvider.GetService<IUserProxy>();

            return userManagerProxy.CreateUser(
                randomUser.FirstName,
                randomUser.LastName,
                randomUser.Email,
                randomUser.UserName,
                randomUser.Password,
                roleToPermissionsList);
        }

        public TestAuthUser CreateRandomUser(
            string firstName = null,
            string lastName = null,
            string email = null,
            string userName = null,
            string password = null,
            Permissions.Permissions[] roles = null)
        {
            var randomUser = this.GetRandomUser();

            randomUser.FirstName = firstName ?? randomUser.FirstName;
            randomUser.LastName = lastName ?? randomUser.LastName;
            randomUser.Email = email ?? randomUser.Email;
            randomUser.UserName = userName ?? randomUser.UserName;
            randomUser.Password = password ?? randomUser.Password;

            if (roles != null)
            {
                randomUser.Permissions = string.Join(',', roles.Select(x => x.ToString()));
            }

            var rolesArray = randomUser.Permissions.Split(',');

            var roleToPermissionsList = rolesArray.Select(s => new RoleToPermissions(Enum.Parse<Permissions.Permissions>(s)))
                .ToList();

            using var scope = this.factory.Services.CreateScope();
            var userManagerProxy = scope.ServiceProvider.GetService<IUserProxy>();

            userManagerProxy.CreateUser(
                randomUser.FirstName,
                randomUser.LastName,
                randomUser.Email,
                randomUser.UserName,
                randomUser.Password,
                roleToPermissionsList);

            return randomUser;
        }

        private static IEnumerable<TestAuthUser> LoadTestUsersFromFile()
        {
            using var streamReader = new StreamReader(TEST_DATA_JSON);
            var json = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<TestAuthUser[]>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }
    }
}
