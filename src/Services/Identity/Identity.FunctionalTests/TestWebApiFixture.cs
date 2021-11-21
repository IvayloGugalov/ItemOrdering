using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

using Identity.API;
using Identity.API.Endpoints.AccountEndpoint;

namespace Identity.FunctionalTests
{
    public class TestWebApiFixture 
    {
        //private readonly MongoDBFixture mongoDbFixture;
        //private HttpClient client;

        //public TestWebApiFixture(MongoDBFixture mongoDbFixture)
        //{
        //    this.mongoDbFixture = mongoDbFixture;
        //    this.client = new TestIdentityWebAppFactory<Startup>().CreateClient();
        //}

        //[Fact]
        //public async Task Test()
        //{
        //    //await this.mongoDbFixture.MongoStorage.UsersToRoles.InsertOneAsync(new UserToRole(new Guid(), new RoleToPermissions("", "", "")));


        //    var body = JsonSerializer.Serialize(
        //        new RegisterRequest
        //        {
        //            FirstName = "Elonk",
        //            LastName = "Musk",
        //            Email = "musk@example.com",
        //            Username = "Martian123",
        //            Role = Permissions.Permissions.Customer,
        //            Password = "420420",
        //            ConfirmPassword = "420420"
        //        });

        //    var content = new StringContent(body, Encoding.UTF8, "application/json");
            

        //    var result = await this.client.PostAsync(RegisterRequest.ROUTE, content);



        //}
    }
   
}
