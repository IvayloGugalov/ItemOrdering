using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Identity.API.Endpoints.AdminEndpoint;
using Identity.Shared;

namespace Identity.Functional.Tests.AdminEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class DeleteUserTest
    {
        private readonly IntegrationTestBase testBase;
        private readonly TestAuthUser adminUser;

        public DeleteUserTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.adminUser = this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task DeleteUser_WillSucceed()
        {
            this.GetCreateUserResponse(out var testUser);

            var deleteData = JsonSerializer.Serialize(
                new DeleteUserRequest
                {
                    Email = testUser.Email
                });

            var deleteUserContent = new StringContent(deleteData, Encoding.UTF8, "application/json");

            var deleteResponse = await this.testBase.Client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    DeleteUserRequest.ROUTE)
                {
                    Content = deleteUserContent
                });

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUser_WhenDeletingYourself_WillFail()
        {
            var deleteData = JsonSerializer.Serialize(
                new DeleteUserRequest
                {
                    Email = this.adminUser.Email
                });

            var deleteUserContent = new StringContent(deleteData, Encoding.UTF8, "application/json");

            var deleteResponse = await this.testBase.Client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    DeleteUserRequest.ROUTE)
                {
                    Content = deleteUserContent
                });

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            deleteResponse.GetErrorMessageFromResponse().Should().BeEquivalentTo("Can't delete yourself!");
        }

        [Fact]
        public async Task DeleteUser_WhichDoesNotExist_WillReturnError()
        {
            var deleteData = JsonSerializer.Serialize(
                new DeleteUserRequest
                {
                    Email = "ala_bala_random_mail@mail.com"
                });

            var deleteUserContent = new StringContent(deleteData, Encoding.UTF8, "application/json");

            var deleteResponse = await this.testBase.Client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    DeleteUserRequest.ROUTE)
                {
                    Content = deleteUserContent
                });

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            deleteResponse.GetErrorMessageFromResponse().Should().BeEquivalentTo("User with this email was not found.");
        }

        private HttpResponseMessage GetCreateUserResponse(out TestAuthUser testUser)
        {
            testUser = this.testBase.UserFactory.GetRandomUser();
            var testUserData = JsonSerializer.Serialize(
                new CreateUserRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Password = testUser.Password,
                    Roles = new[] { ((Permissions.Permissions)int.Parse(testUser.Permissions)).ToString() }
                });

            var createUserContent = new StringContent(testUserData, Encoding.UTF8, "application/json");

            return this.testBase.Client.PostAsync(CreateUserRequest.ROUTE, createUserContent)
                .GetAwaiter().GetResult();
        }
    }
}
