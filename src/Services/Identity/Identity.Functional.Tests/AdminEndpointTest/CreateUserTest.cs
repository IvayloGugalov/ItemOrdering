using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using MongoDB.Driver;
using Xunit;

using Identity.API.Endpoints.AdminEndpoint;
using Identity.Domain.Entities;
using Identity.Shared;

namespace Identity.Functional.Tests.AdminEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class CreateUserTest
    {
        private readonly IntegrationTestBase testBase;

        public CreateUserTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task CreateUser_WillSucceed()
        {
            var createUserResponse = this.GetCreateUserResponse(out var testUser);

            createUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await this.testBase.MongoStorage.Users.FindAsync(
                Builders<AuthUser>.Filter.Eq(x => x.Email, testUser.Email));
            user.SingleOrDefault().Email.Should().BeEquivalentTo(testUser.Email);
        }

        [Fact]
        public void CreateUser_WithWrongRole_WillFail()
        {
            var createUserResponse = this.GetCreateUserResponse(out _, "Non Existent Role");

            createUserResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
            createUserResponse.GetErrorMessageFromResponse().Should().BeEquivalentTo("No such roles to add for user");
        }

        private HttpResponseMessage GetCreateUserResponse(out TestAuthUser testUser, string testUserRole = null)
        {
            testUser = this.testBase.UserFactory.GetRandomUser();
            var role = string.IsNullOrEmpty(testUserRole)
                ? ((Permissions.Permissions)int.Parse(testUser.Permissions)).ToString()
                : testUserRole;
            var testUserData = JsonSerializer.Serialize(
                new CreateUserRequest
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Email = testUser.Email,
                    Username = testUser.UserName,
                    Password = testUser.Password,
                    Roles = new[] { role }
                });

            var createUserContent = new StringContent(testUserData, Encoding.UTF8, "application/json");

            return this.testBase.Client.PostAsync(CreateUserRequest.ROUTE, createUserContent)
                .GetAwaiter().GetResult();
        }
    }
}
