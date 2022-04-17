using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using HttpClientExtensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using Identity.API;
using Identity.API.Endpoints.AccountEndpoint;
using Identity.Functional.Tests.EntityBuilders;
using Identity.Infrastructure.MongoDB.Storages;

namespace Identity.Functional.Tests
{
    [CollectionDefinition(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class Base : ICollectionFixture<IntegrationTestBase> { }

    public class IntegrationTestBase : IDisposable
    {
        public const string TEST_COLLECTION_NAME = "IntegrationCollection";

        public HttpClient Client { get; }
        public IMongoStorage MongoStorage { get; }
        public TestIdentityWebAppFactory<Startup> Factory { get; }
        public UserFactory UserFactory { get; }

        public IntegrationTestBase()
        {
            this.Factory = new TestIdentityWebAppFactory<Startup>();
            this.Client = this.Factory.CreateClient();
            this.MongoStorage = this.Factory.Services.GetService<IMongoStorage>();
            this.UserFactory = new UserFactory(this.Factory);
        }

        public TestAuthUser AddAuthorization(Permissions.Permissions permission)
        {
            var user = this.UserFactory.CreateRandomUser(roles: new [] { permission });

            var body = JsonSerializer.Serialize(new LoginRequest { Email = user.Email, Password = user.Password });
            var loginContent = new StringContent(body, Encoding.UTF8, "application/json");

            var (accessToken, _) = this.Client.PostAndReceiveResult<UserAuthenticatedDto>(LoginRequest.ROUTE, loginContent).GetAwaiter().GetResult();

            this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return user;
        }

        public void Dispose()
        {
            this.MongoStorage.Client.DropDatabase(this.MongoStorage.DatabaseName);
            this.Client?.Dispose();
        }
    }
}
