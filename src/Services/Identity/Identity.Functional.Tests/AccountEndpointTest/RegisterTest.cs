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

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var testUserWithSameName = this.testBase.GetRandomUser();
            var newData = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUserWithSameName.FirstName,
                    LastName = testUser.LastName,
                    Email = testUserWithSameName.Email,
                    Username = testUser.UserName, // Same userName
                    Role = Enum.Parse<Permissions.Permissions>(testUserWithSameName.Permissions),
                    Password = testUserWithSameName.Password,
                    ConfirmPassword = testUserWithSameName.Password
                });

            content = new StringContent(newData, Encoding.UTF8, "application/json");

            var responseWithError = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            // Assert
            responseWithError.StatusCode.Should().Be(HttpStatusCode.Conflict);
            (await responseWithError.Content.ReadAsStringAsync()).Should().Contain("errorMessage");
        }
    }
}
