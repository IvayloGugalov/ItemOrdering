using System;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

using Identity.API;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.FunctionalTests
{
    [CollectionDefinition(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class Base : ICollectionFixture<IntegrationTestBase> { }

    public class IntegrationTestBase : IDisposable
    {
        public const string TEST_COLLECTION_NAME = "IntegrationTestBase";

        public HttpClient Client { get; }
        public IMongoStorage MongoStorage { get; }
        public TestIdentityWebAppFactory<Startup> Factory { get; }

        public IntegrationTestBase()
        {
            this.Factory = new TestIdentityWebAppFactory<Startup>();
            this.Client = this.Factory.CreateClient();
            this.MongoStorage = this.Factory.Services.GetService<IMongoStorage>();
        }

        public void Dispose()
        {
            this.MongoStorage.Client.DropDatabase(this.MongoStorage.DatabaseName);
            this.Client?.Dispose();
        }
    }
}
