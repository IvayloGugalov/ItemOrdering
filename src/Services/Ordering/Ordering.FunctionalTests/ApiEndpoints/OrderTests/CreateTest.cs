using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using HttpClientExtensions;

using Ordering.API;
using Ordering.API.Endpoints.OrderEndpoint;
using Ordering.Domain.OrderAggregate.Specifications;
using Ordering.Domain.Shared;
using Ordering.Infrastructure.Data;

namespace Ordering.Functional.Tests.ApiEndpoints.OrderTests
{
    [TestFixture]
    public class CreateTest
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
        public async Task Create_WillSucceed()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = Seeder.CustomerWithCartAndProducts(dbContext);

            var result = await this.httpClient.PostAndReceiveMessage(CreateOrderRequest.BuildRoute(customer.Id));

            var order = await dbContext.Orders.Specify(new OrderWithProductsSpec(customer.Id)).SingleOrDefaultAsync();
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual(customer.Id, order.CustomerId);
        }
    }
}
