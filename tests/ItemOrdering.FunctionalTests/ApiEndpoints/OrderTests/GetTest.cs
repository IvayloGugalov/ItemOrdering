using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.FunctionalTests.HttpExtension;
using ItemOrdering.Infrastructure.Data;
using ItemOrdering.Web;
using ItemOrdering.Web.Endpoints.OrderEndpoint;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ItemOrdering.FunctionalTests.ApiEndpoints.OrderTests
{
    [TestFixture]
    public class GetTest
    {
        private HttpClient httpClient;
        private TestWebAppFactory<Startup> app;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.app = new TestWebAppFactory<Startup>();
            this.httpClient = this.app.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.httpClient.Dispose();
            this.app.Dispose();
        }

        [Test]
        public async Task Get_WillSucceed()
        {
            //using var scope = this.app.Services.CreateScope();
            //var services = scope.ServiceProvider;
            //var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            //var customer = Seeder.CustomerWithMultipleOrders(dbContext, 5);

            //var result = await this.httpClient.GetDeserializedJsonResult<GetOrdersResponse>(GetOrdersRequest.BuildRoute(customer.Id));

            //var result = await this.httpClient.PostAndReceiveMessage(CreateOrderRequest.BuildRoute(customer.Id));

            //var order = await dbContext.Orders.GetProductsForOrder(customer.Id).SingleOrDefaultAsync();
            //Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            //Assert.AreEqual(customer.Id, order.CustomerId);
        }
    }
}
