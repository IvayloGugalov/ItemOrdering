using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;
using HttpClientExtensions;
using Xunit;

using Identity.API.Endpoints.AccountEndpoint;
using Identity.FunctionalTests.EntityBuilders;

namespace Identity.FunctionalTests.AccountEndpointTest
{
    [Collection(IntegrationTestBase.TEST_COLLECTION_NAME)]
    public class PasswordResetTest
    {
        private readonly IntegrationTestBase testBase;

        private const string password = "420420";
        private const string email = "mus6k@example.com";
        private const string userName = "Doge123";

        public PasswordResetTest(IntegrationTestBase testBase)
        {
            this.testBase = testBase;
        }

        [Fact]
        public async Task PasswordResetTest_WillSucceed()
        {
            AuthUserCreator.Create(
                "Elonk",
                "Musk",
                PasswordResetTest.email,
                PasswordResetTest.userName,
                PasswordResetTest.password,
                Permissions.Permissions.Customer,
                this.testBase.Factory);

            var newPassword = "69696969";
            var data = JsonSerializer.Serialize(
                new PasswordResetRequest
                {
                    NewPassword = newPassword,
                    ConfirmNewPassword = newPassword,
                    Email = PasswordResetTest.email
                });
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await this.testBase.Client.PostAndReceiveMessage(PasswordResetRequest.ROUTE, content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = JsonSerializer.Serialize(
                new LoginRequest { Username = PasswordResetTest.userName, Password = newPassword });

            var loginContent = new StringContent(body, Encoding.UTF8, "application/json");

            var loginResponse = await this.testBase.Client.PostAndReceiveMessage(LoginRequest.ROUTE, loginContent);

            //Assert
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
