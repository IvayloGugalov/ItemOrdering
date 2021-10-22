using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using ItemOrdering.Api;
using ItemOrdering.Api.Endpoints.ShoppingCartEndpoint;
using ItemOrdering.FunctionalTests.HttpExtension;
using ItemOrdering.Infrastructure.Data;

namespace ItemOrdering.FunctionalTests.ApiEndpoints.ShoppingCartTests
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

            var customer = dbContext.Customers.FirstOrDefault();
            if (customer is null) throw new ArgumentNullException(nameof(customer), "No customer created during seed operation.");

            var result = await this.httpClient.PostAndReceiveResult<CreateShoppingCartResponse>(
                CreateShoppingCartRequest.BuildRoute(customer.Id));

            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.IsTrue(dbContext.ShoppingCarts.Any(x => x.Id == result.Id));
        }
    }
}
