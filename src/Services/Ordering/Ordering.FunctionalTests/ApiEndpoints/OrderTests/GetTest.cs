using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using HttpClientExtensions;

using Ordering.API;
using Ordering.API.Endpoints.OrderEndpoint;
using Ordering.Infrastructure.Data;
using GuidGenerator;

namespace Ordering.Functional.Tests.ApiEndpoints.OrderTests
{
    [TestFixture]
    public class GetTest
    {
        private HttpClient httpClient;
        private TestWebAppFactory<Startup> app;
        private IGuidGeneratorService guidGenerator;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.app = new TestWebAppFactory<Startup>();
            this.httpClient = this.app.CreateClient();
            this.guidGenerator = new GuidGeneratorService();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.httpClient.Dispose();
            this.app.Dispose();
        }

        [Test]
        public async Task Get_WithMultipleOrders_WillSucceed()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = Seeder.CustomerWithMultipleOrders(dbContext, this.guidGenerator, orderCount: 5);

            var result = await this.httpClient.GetDeserializedJsonResult<GetOrdersResponse>(GetOrdersRequest.BuildRoute(customer.Id));

            var customerOrdersById = customer.Orders.Select(x => x.Id);
            var resultOrdersById = result.OrdersDto.Select(x => x.Id);

            Assert.AreEqual(customer.Orders.Count, result.OrdersDto.Count);
            Assert.IsTrue(customerOrdersById.SequenceEqual(resultOrdersById));
        }

        [Test]
        public async Task Get_ForCustomerWithNoOrders_WillReturnNoContent()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = Seeder.CustomerWithCartAndProducts(dbContext, this.guidGenerator);

            var result = await this.httpClient.GetAsync(GetOrdersRequest.BuildRoute(customer.Id));

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task Get_ForSingleOrder_WillSucceed()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = Seeder.CustomerWithMultipleOrders(dbContext, this.guidGenerator, orderCount: 1);

            var order = customer.Orders.First();

            var result = await this.httpClient.GetDeserializedJsonResult<GetOrderResponse>(GetOrderRequest.BuildRoute(customer.Id, order.Id));

            Assert.AreEqual(order.Id, result.OrderDto.Id);
        }
    }
}
