using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

using Identity.API;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.Functional.Tests
{
    [CollectionDefinition(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class Base : ICollectionFixture<IntegrationTestBase> { }

    public class IntegrationTestBase : IDisposable
    {
        private static readonly string TEST_DATA_JSON =
            Path.Combine(
                Path.GetFullPath(
                    Path.Combine("..", "..", "..", "..", "Identity.Functional.Tests")),
                "test_data.json");

        private readonly Random random = new ();

        public const string TEST_COLLECTION_NAME = "IntegrationCollection";

        public HttpClient Client { get; }
        public IMongoStorage MongoStorage { get; }
        public TestIdentityWebAppFactory<Startup> Factory { get; }

        /// <summary>
        /// 500 randomly generated users
        /// </summary>
        public TestAuthUser[] TestUsers { get; }

        public IntegrationTestBase()
        {
            this.Factory = new TestIdentityWebAppFactory<Startup>();
            this.Client = this.Factory.CreateClient();
            this.MongoStorage = this.Factory.Services.GetService<IMongoStorage>();

            this.TestUsers = LoadTestUsersFromFile().ToArray();
        }

        public TestAuthUser GetRandomUser() => this.TestUsers[random.Next(this.TestUsers.Length - 1)];

        public void Dispose()
        {
            this.MongoStorage.Client.DropDatabase(this.MongoStorage.DatabaseName);
            this.Client?.Dispose();
        }

        private static IEnumerable<TestAuthUser> LoadTestUsersFromFile()
        {
            using var streamReader = new StreamReader(IntegrationTestBase.TEST_DATA_JSON);
            var json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<TestAuthUser[]>(json, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Include });
        }
    }
}
