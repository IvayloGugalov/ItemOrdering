using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AdminEndpoint;
using Identity.Permissions;

namespace Identity.Functional.Tests.AdminEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class GetRolesTest
    {
        private readonly IntegrationTestBase testBase;

        public GetRolesTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task GetRoles_WillSucceed()
        {
            var expectedRoles = await this.testBase.Client.GetDeserializedJsonResult<GetRolesResponse>(GetRolesRequest.ROUTE);

            var actualRoles = Enum.GetValues<Permissions.Permissions>();

            _ = expectedRoles.Roles.Select(roleString => actualRoles.Should().ContainSingle(role => role.GetDisplayName() == roleString));
        }
    }
}
