using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.API.Extensions;
using Identity.Shared;

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
            var testUser = this.testBase.UserFactory.CreateRandomUser();

            var body = JsonSerializer.Serialize(
                new LoginRequest { Email = testUser.Email, Password = testUser.Password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAsync(LoginRequest.ROUTE, content);

            var refreshResponse = await this.testBase.Client.SendAsync(
                CookieExtensionMethods.SetCookieOnRequest(RefreshRequest.ROUTE, loginResponse, HttpMethod.Get, AppendCookieExtension.REFRESH_TOKEN_NAME));

            refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var authenticatedUser = await refreshResponse.Content.ReadFromJsonAsync<UserAuthenticatedDto>();
            authenticatedUser.Should().NotBeNull();
            authenticatedUser.Roles.Should().NotBeEmpty();
            authenticatedUser.AccessToken.Should().NotBeEmpty();
        }

        [Fact]
        public async Task RefreshTest_WithNoRefreshTokenCookie_WillReturnUnauthorized()
        {
            var refreshResponse = await this.testBase.Client.GetAsync(RefreshRequest.ROUTE);
            var error = await refreshResponse.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            error.ErrorMessages.Should().NotBeEmpty();
            error.ErrorMessages.First().Should().Be("No refresh token found");
        }
    }
}
