using System;
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
    public class UpdateRoleTest
    {
        private readonly IntegrationTestBase testBase;

        public UpdateRoleTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task UpdateRole_WillSucceed()
        {
            var roleName = "Customer";
            var description = "Can work in a shop as an employee";
            var permissionName = new[] { Permissions.Permissions.ShopEmployee.ToString() };
            var permissionChar = (char)Convert.ChangeType(Permissions.Permissions.ShopEmployee, typeof(char));

            var roleData = JsonSerializer.Serialize(
                new CreateRoleRequest
                {
                    RoleName = roleName,
                    Description = description,
                    PermissionNames = permissionName
                });

            var content = new StringContent(roleData, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PutAsync(UpdateRoleRequest.ROUTE, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedRole = await response.Content.ReadFromJsonAsync<UpdateRoleResponse>();

            updatedRole.RoleToPermission.PackedPermissions.Should().Contain(permissionChar.ToString());
        }
    }
}
