using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.Functional.Tests.EntityBuilders;
using Identity.Tokens;
using Identity.Tokens.Interfaces;

namespace Identity.Functional.Tests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class LogoutTest
    {
        private readonly IntegrationTestBase testBase;

        public LogoutTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task LogoutTest_WillSucceed()
        {
            var testUser = this.testBase.GetRandomUser();
            AuthUserCreator.Create(testUser, this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Email = testUser.Email, Password = testUser.Password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAsync(LoginRequest.ROUTE, content);

            var (accessToken, _) = await loginResponse.Content.ReadFromJsonAsync<UserAuthenticatedDto>() ?? throw new NullReferenceException(nameof(UserAuthenticatedDto));

            using var scope = this.testBase.Factory.Services.CreateScope();
            var accessTokenValidator = scope.ServiceProvider.GetService<IAccessTokenValidator>();

            accessTokenValidator.Validate(accessToken, out _)
                .Should().Be(TokenValidationResult.Success);

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var logoutResponse = await this.testBase.Client.DeleteAsync(LogoutRequest.ROUTE);

            // Assert
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // TODO: Try to verify cookie got deleted
            //logoutResponse.Headers.SingleOrDefault(x => x.Key == "Set-Cookie").Value.FirstOrDefault(x => x.Name == "refresh_token).Should.BeNull();
        }
    }
}
