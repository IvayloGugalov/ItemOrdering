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
using Identity.Functional.Tests.EntityBuilders;

namespace Identity.Functional.Tests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class DeleteTest
    {
        private readonly IntegrationTestBase testBase;

        public DeleteTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task DeleteTest_WillSucceed()
        {
            var testUser = this.testBase.GetRandomUser();
            AuthUserCreator.Create(testUser, this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Email = testUser.Email, Password = testUser.Password });

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
            var testUser = this.testBase.GetRandomUser();
            AuthUserCreator.Create(testUser, this.testBase.Factory);

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "SomeRandomTokenValue");

            var response = await this.testBase.Client.DeleteAsync(DeleteRequest.ROUTE);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
