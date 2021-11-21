using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Identity.API;
using Identity.API.Endpoints.AccountEndpoint;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    public class RegisterTest : IntegrationTestBase
    {
        public RegisterTest(TestIdentityWebAppFactory<Startup> factory)
            : base(factory) { }


        [Fact]
        public async Task RegisterTest_WillSucceed()
        {
            var data = JsonSerializer.Serialize(
                new RegisterRequest
                {
                    FirstName = "Elonk",
                    LastName = "Musk",
                    Email = "musk@example.com",
                    Username = "Martian123",
                    Role = Permissions.Permissions.Customer,
                    Password = "420420",
                    ConfirmPassword = "420420"
                });

            var content = new StringContent(data, Encoding.UTF8, "application/json");


            var result = await this.client.PostAsync(RegisterRequest.ROUTE, content);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
