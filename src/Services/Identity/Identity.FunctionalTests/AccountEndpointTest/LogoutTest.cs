using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.FunctionalTests.EntityBuilders;
using Identity.Infrastructure.MongoDB.Storages;
using Identity.Tokens.Tokens;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class LogoutTest
    {
        private readonly IntegrationTestBase testBase;

        private const string userName = "M5artian123";
        private const string password = "420420";

        public LogoutTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task LogoutTest_WillSucceed()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "mu35sk@example.com",
                LogoutTest.userName,
                LogoutTest.password,
                Permissions.Permissions.NotSet,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = LogoutTest.userName, Password = LogoutTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var (accessToken, refreshToken) = await this.testBase.Client.PostAndReceiveResult<UserAuthenticatedDto>(LoginRequest.ROUTE, content);

            using var scope = this.testBase.Factory.Services.CreateScope();
            var mongoStorage = scope.ServiceProvider.GetService<IMongoStorage>();

            var refreshTokenFromDb = await mongoStorage.RefreshTokens
                .Find(Builders<RefreshToken>.Filter.Eq(x => x.TokenValue, refreshToken))
                .SingleOrDefaultAsync();

            refreshTokenFromDb.TokenValue.Should().Be(refreshToken);

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await this.testBase.Client.DeleteAsync(LogoutRequest.ROUTE);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            (await mongoStorage.RefreshTokens
                .Find(Builders<RefreshToken>.Filter.Eq(x => x.TokenValue, refreshToken))
                .SingleOrDefaultAsync())
                .Should().Be(null);
        }
    }
}
