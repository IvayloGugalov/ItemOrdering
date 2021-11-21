using System;
using System.Net.Http;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

using Identity.API;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.FunctionalTests
{
    public class IntegrationTestBase : IClassFixture<TestIdentityWebAppFactory<Startup>>, IDisposable
    {
        internal readonly HttpClient client;
        internal readonly IMongoStorage mongoStorage;

        protected IntegrationTestBase(TestIdentityWebAppFactory<Startup> factory)
        {
            this.client = factory.CreateClient();
            this.mongoStorage = factory.Services.GetService<IMongoStorage>();
        }

        public void Dispose()
        {
            this.mongoStorage.Client.DropDatabase(this.mongoStorage.DatabaseName);
            this.client?.Dispose();
        }
    }
}
