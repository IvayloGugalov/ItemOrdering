using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.FunctionalTests.EntityBuilders;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class RefreshTest
    {
        private readonly IntegrationTestBase testBase;

        private const string userName = "Ma1223122rtian123";
        private const string password = "420420";

        public RefreshTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task RefreshTest_WillSucceed()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "mu0sk@example.com",
                RefreshTest.userName,
                RefreshTest.password,
                Permissions.Permissions.ShopOwner,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = RefreshTest.userName, Password = RefreshTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var (_, refreshToken) = await this.testBase.Client.PostAndReceiveResult<UserAuthenticatedDto>(LoginRequest.ROUTE, content);

            var refreshBody = JsonSerializer.Serialize(
                new RefreshRequest { RefreshTokenValue = refreshToken });
            var refreshContent = new StringContent(refreshBody, Encoding.UTF8, "application/json");

            var refreshResponse = await this.testBase.Client.PostAndReceiveMessage(RefreshRequest.ROUTE, refreshContent);

            // Assert
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
