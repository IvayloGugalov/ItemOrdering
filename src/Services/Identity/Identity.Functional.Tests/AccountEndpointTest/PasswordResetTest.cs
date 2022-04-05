using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.Functional.Tests.EntityBuilders;

namespace Identity.Functional.Tests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class PasswordResetTest
    {
        private readonly IntegrationTestBase testBase;

        public PasswordResetTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task PasswordResetTest_WillSucceed()
        {
            var testUser = this.testBase.GetRandomUser();
            AuthUserCreator.Create(testUser, this.testBase.Factory);

            var newPassword = "69696969";
            var data = JsonSerializer.Serialize(
                new PasswordResetRequest
                {
                    NewPassword = newPassword,
                    ConfirmNewPassword = newPassword,
                    Email = testUser.Email
                });
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAndReceiveMessage(PasswordResetRequest.ROUTE, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Email = testUser.Email, Password = newPassword });

            var loginContent = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAndReceiveMessage(LoginRequest.ROUTE, loginContent);

            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
