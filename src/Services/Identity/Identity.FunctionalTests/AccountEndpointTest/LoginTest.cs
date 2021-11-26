using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.FunctionalTests.EntityBuilders;
using Identity.Tokens;
using Identity.Tokens.Interfaces;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    [Collection("IntegrationTestBase")]
    public class LoginTest
    {
        private readonly IntegrationTestBase testBase;

        private const string userName = "Martia66n123";
        private const string email = "m66usk@example.com";
        private const string password = "420420";

        public LoginTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }
        
        [Fact]
        public async Task LoginTest_WillSucceed()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                LoginTest.email,
                LoginTest.userName,
                LoginTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = LoginTest.userName, Password = LoginTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var (accessToken, _) = await this.testBase.Client.PostAndReceiveResult<UserAuthenticatedDto>(LoginRequest.ROUTE, content);

            // Assert
            using var scope = this.testBase.Factory.Services.CreateScope();
            var accessTokenValidator = scope.ServiceProvider.GetService<IAccessTokenValidator>();

            accessTokenValidator.Validate(accessToken, out _)
                .Should().Be(TokenValidationResult.Success);
        }

        [Fact]
        public async Task LoginTest_WithWrongUserName_WillFail()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "mus77k@example.com",
                "66546 name",
                LoginTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = LoginTest.userName + "..", Password = LoginTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAsync(LoginRequest.ROUTE, content);

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task LoginTest_WithWrongPassword_WillFail()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                "m334usk@example.com",
                "123 name",
                LoginTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = LoginTest.userName, Password = LoginTest.password + ".." });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAsync(LoginRequest.ROUTE, content);

            // Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // TODO: Remove with it's calling method
        [Fact]
        public void Test_Secret_WithPermission()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                LoginTest.email,
                LoginTest.userName,
                LoginTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);;

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = LoginTest.userName, Password = LoginTest.password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = this.testBase.Client.PostAsync(LoginRequest.ROUTE, content).GetAwaiter().GetResult();
            var (accessToken, _) = loginResponse.Content.ReadFromJsonAsync<UserAuthenticatedDto>().GetAwaiter().GetResult();

            this.testBase.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = this.testBase.Client.GetAsync("api/login/secret").GetAwaiter().GetResult();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                .Should().Contain("customer secret");
        }
    }
}
