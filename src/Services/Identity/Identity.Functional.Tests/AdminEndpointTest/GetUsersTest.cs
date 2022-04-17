using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AdminEndpoint;

namespace Identity.Functional.Tests.AdminEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class GetUsersTest
    {
        private readonly IntegrationTestBase testBase;

        public GetUsersTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task GetUsers_WillSucceed()
        {
            var user1 = this.testBase.UserFactory.CreateRandomUser();
            var user2 = this.testBase.UserFactory.CreateRandomUser();

            var expectedUsers = await this.testBase.Client.GetDeserializedJsonResult<List<UserDto>>(GetUsersRequest.ROUTE);

            expectedUsers.Should().Contain(u => u.Email == user1.Email);
            expectedUsers.Should().Contain(u => u.UserName == user1.UserName);
            expectedUsers.Should().Contain(u => u.Email == user2.Email);
            expectedUsers.Should().Contain(u => u.UserName == user2.UserName);
        }
    }
}
