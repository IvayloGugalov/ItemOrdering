using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.Functional.Tests.EntityBuilders;

namespace Identity.Functional.Tests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class RefreshTest
    {
        private readonly IntegrationTestBase testBase;

        public RefreshTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task RefreshTest_WillSucceed()
        {
            var testUser = this.testBase.GetRandomUser();
            AuthUserCreator.Create(testUser, this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = testUser.UserName, Password = testUser.Password });

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
