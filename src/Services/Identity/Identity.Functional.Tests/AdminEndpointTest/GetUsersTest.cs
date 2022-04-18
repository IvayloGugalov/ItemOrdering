using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FluentAssertions;
using GuidGenerator;
using HttpClientExtensions;
using Moq;
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
            var guidGeneratorMock = new Mock<IGuidGeneratorService>();

            guidGeneratorMock.SetupSequence(_ => _.GenerateGuid())
                .Returns(Guid.Parse("0F0204DD-019B-4EB9-B959-3515B078BCBA"))
                .Returns(Guid.Parse("779F4920-ADF9-4CF8-9DD6-DE8FB1BC7D8F"));

            var user1 = this.testBase.UserFactory.CreateRandomUser(guidGenerator: guidGeneratorMock.Object);
            var user2 = this.testBase.UserFactory.CreateRandomUser(guidGenerator: guidGeneratorMock.Object);

            var expectedUsers = await this.testBase.Client.GetDeserializedJsonResult<List<UserDto>>(GetUsersRequest.ROUTE);

            expectedUsers.Should().Contain(u => u.Email == user1.Email && u.UserName == user1.UserName);
            expectedUsers.Should().Contain(u => u.Email == user2.Email && u.UserName == user2.UserName);
        }
    }
}
