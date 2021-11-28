using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;

namespace Identity.Functional.Tests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class RegisterTest
    {
        private readonly IntegrationTestBase testBase;
        private string registeredUserName;

        public RegisterTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task RegisterTest_WillSucceed()
        {
            var testUser = this.testBase.GetRandomUser();
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Role = Enum.Parse<Permissions.Permissions>(testUser.Permissions),
                    Password = testUser.Password,
                    ConfirmPassword = testUser.Password
                });
            this.registeredUserName = testUser.UserName;

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterTest_OnInvalidRole_WillReturnABadRequest()
        {
            var testUser = this.testBase.GetRandomUser();
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Role = (Permissions.Permissions)1,
                    Password = testUser.Password,
                    ConfirmPassword = testUser.Password
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
                    Username = this.registeredUserName,
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
