using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Identity.API;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.Shared;

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
            var testUser = this.testBase.UserFactory.GetRandomUser();
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Password = testUser.Password,
                    ConfirmPassword = testUser.Password
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterTest_OnDuplicateUserName_WillReturnConflict()
        {
            var testUser = this.testBase.UserFactory.GetRandomUser();
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Password = testUser.Password,
                    ConfirmPassword = testUser.Password
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var testUserWithSameName = this.testBase.UserFactory.GetRandomUser();
            var newData = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUserWithSameName.FirstName,
                    LastName = testUser.LastName,
                    Email = testUserWithSameName.Email,
                    Username = testUser.UserName, // Same userName
                    Password = testUserWithSameName.Password,
                    ConfirmPassword = testUserWithSameName.Password
                });

            content = new StringContent(newData, Encoding.UTF8, "application/json");

            var responseWithError = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            // Assert
            responseWithError.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var errorMessages = (await responseWithError.Content.ReadFromJsonAsync<ErrorResponse>()).ErrorMessages;
            errorMessages.ToList().FirstOrDefault().Should().BeEquivalentTo(ErrorMessages.USERNAME_EXISTS);
        }

        [Fact]
        public async Task RegisterTest_OnDuplicateEmail_WillReturnConflict()
        {
            var testUser = this.testBase.UserFactory.GetRandomUser();
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Password = testUser.Password,
                    ConfirmPassword = testUser.Password
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var testUserWithSameName = this.testBase.UserFactory.GetRandomUser();
            var newData = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = testUserWithSameName.FirstName,
                    LastName = testUserWithSameName.LastName,
                    Email = testUser.Email, // Same email
                    Username = testUserWithSameName.UserName, 
                    Password = testUserWithSameName.Password,
                    ConfirmPassword = testUserWithSameName.Password
                });

            content = new StringContent(newData, Encoding.UTF8, "application/json");

            var responseWithError = await this.testBase.Client.PostAsync(RegisterRequest.ROUTE, content);
            // Assert
            responseWithError.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var errorMessages = (await responseWithError.Content.ReadFromJsonAsync<ErrorResponse>()).ErrorMessages;
            errorMessages.ToList().FirstOrDefault().Should().BeEquivalentTo(ErrorMessages.EMAIL_EXISTS);
        }
    }
}
