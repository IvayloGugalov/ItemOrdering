using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class RegisterTest
    {
        private readonly IntegrationTestBase testBase;

        public RegisterTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task RegisterTest_WillSucceed()
        {
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = "Elonk",
                    LastName = "Musk",
                    Email = "mu3sk@example.com",
                    Username = "M3artian123",
                    Role = Permissions.Permissions.Customer,
                    Password = "420420",
                    ConfirmPassword = "420420"
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterTest_OnInvalidRole_WillReturnABadRequest()
        {
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = "Elonk",
                    LastName = "Musk",
                    Email = "m1usk@example.com",
                    Username = "Ma1rtian123",
                    Role = (Permissions.Permissions)1,
                    Password = "420420",
                    ConfirmPassword = "420420"
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // TODO: Convert messages to constants and test DuplicateEmail
        [Fact]
        public async Task RegisterTest_OnDuplicateUserName_WillReturnConflict()
        {
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = "Elonk",
                    LastName = "Musk",
                    Email = "mu2sk@example.com",
                    Username = "M2artian123",
                    Role = Permissions.Permissions.Customer,
                    Password = "420420",
                    ConfirmPassword = "420420"
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            (await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content))
                .StatusCode.Should().Be(HttpStatusCode.OK);

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            (await response.Content.ReadAsStringAsync()).Should().Contain("errorMessage");
        }
    }
}
