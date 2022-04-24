using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using HttpClientExtensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using Ordering.API;
using Ordering.API.Endpoints.ShoppingCartEndpoint;
using Ordering.Infrastructure.Data;

namespace Ordering.FunctionalTests.ApiEndpoints.ShoppingCartTests
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
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = dbContext.Customers.FirstOrDefault();
            if (customer is null) throw new ArgumentNullException(nameof(customer), "No customer created during seed operation.");

            var result = await this.httpClient.GetDeserializedJsonResult<GetShoppingCartResponse>(
                GetShoppingCartRequest.BuildRoute(customer.Id));

            Assert.IsTrue(result.ShoppingCartDto.Id != Guid.Empty);
        }

        [Test]
        public async Task Get_WhenNoCartExists_WillCreateAShoppingCart()
        {
            var result = await this.httpClient.GetDeserializedJsonResult<GetShoppingCartResponse>(
                GetShoppingCartRequest.BuildRoute(Guid.NewGuid()));

            Assert.IsTrue(result.ShoppingCartDto.Id != Guid.Empty);
        }

        [Test, Ignore("Should this be done?")]
        public async Task Get_WithWrongCustomerId_WillReturnNotFound()
        {
            var result = await this.httpClient.GetAndEnsureNotFound(GetShoppingCartRequest.BuildRoute(Guid.NewGuid()));

            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
