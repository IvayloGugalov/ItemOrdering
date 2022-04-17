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
    public class CreateRoleTest
    {
        private readonly IntegrationTestBase testBase;

        public CreateRoleTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;

            this.testBase.AddAuthorization(Permissions.Permissions.Admin);
        }

        [Fact]
        public async Task CreateRole_WillSucceed()
        {
            var roleName = "Office Manager";
            var description = "The manager of an office";
            var permissionName = new[] { Permissions.Permissions.Admin.ToString() };
            var permissionChar = (char)Convert.ChangeType(Permissions.Permissions.Admin, typeof(char));

            var roleData = JsonSerializer.Serialize(
                new CreateRoleRequest
                {
                    RoleName = roleName,
                    Description = description,
                    PermissionNames = permissionName
                });

            var content = new StringContent(roleData, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAsync(CreateRoleRequest.ROUTE, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdRole = await response.Content.ReadFromJsonAsync<CreateRoleResponse>();

            createdRole.RoleToPermission.RoleName.Should().Be(roleName);
            createdRole.RoleToPermission.PackedPermissions.Should().Be(permissionChar.ToString());
        }
    }
}
