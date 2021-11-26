using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class DeleteTest
    {
        private readonly IntegrationTestBase testBase;

        private const string userName = "Marti11234an123";
        private const string password = "420420";

        public DeleteTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task DeleteTest_WillSucceed()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "musk5532@example.com",
                DeleteTest.userName,
                DeleteTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = DeleteTest.userName, Password = DeleteTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var (accessToken, _) = await this.testBase.Client.PostAndReceiveResult<UserAuthenticatedDto>(LoginRequest.ROUTE, content);

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await this.testBase.Client.DeleteAsync(DeleteRequest.ROUTE);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteTest_WithUnauthorizedUser_WillFail()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "235musk@example.com",
                "32436 Nameee",
                DeleteTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "SomeRandomTokenValue");

            var response = await this.testBase.Client.DeleteAsync(DeleteRequest.ROUTE);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
