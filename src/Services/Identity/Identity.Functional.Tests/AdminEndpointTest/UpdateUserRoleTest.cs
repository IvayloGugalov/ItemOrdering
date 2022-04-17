using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Identity.API.Endpoints.AdminEndpoint;

namespace Identity.Functional.Tests.AdminEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class UpdateUserRoleTest
    {
        private readonly IntegrationTestBase testBase;

        public UpdateUserRoleTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task UpdateUserRole_WillSucceed()
        {
            var user = this.testBase.UserFactory.CreateAuthUser();
            var newRole = Permissions.Permissions.ShopOwner.ToString();

            user.UserRoles.Should().NotContain(x => x.Role.RoleName == newRole);

            var requestData = JsonSerializer.Serialize(
                new UpdateUserRoleRequest
                {
                    UserId = user.Id,
                    Role = newRole
                });

            var updateUserRoleContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PutAsync(UpdateUserRoleRequest.ROUTE, updateUserRoleContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<UpdateUserRoleResponse>();

            result.User.Roles.Should().Contain(user.UserRoles.Select(x => x.RoleName));
            result.User.Roles.Should().Contain(newRole);
        }
    }
}
