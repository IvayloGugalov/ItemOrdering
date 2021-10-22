using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using ItemOrdering.Api;
using ItemOrdering.Api.Endpoints.ShoppingCartEndpoint;
using ItemOrdering.Infrastructure.Data;
using ItemOrdering.FunctionalTests.HttpExtension;

namespace ItemOrdering.FunctionalTests.ApiEndpoints.ShoppingCartTests
{
    [TestFixture]
    public class UpdateTest
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
        public async Task Update_WillSucceed()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = dbContext.Customers.FirstOrDefault();
            if (customer is null) throw new ArgumentNullException(nameof(customer), "No customer created during seed operation.");

            var product = dbContext.Products.FirstOrDefault();
            if (product is null) throw new ArgumentNullException(nameof(product), "No product created during seed operation.");

            var body = JsonSerializer.Serialize(
                new UpdateShoppingCartRequest { ProductId = product.Id });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var result = await this.httpClient.PutAndReceiveDeserializedJsonResult<UpdateShoppingCartResponse>(UpdateShoppingCartRequest.BuildRoute(customer.Id), content);

            Assert.IsNotNull(result.ShoppingCart);
            Assert.AreEqual(result.ShoppingCart.ProductsAndAmount.Count, 1);
            Assert.AreEqual(result.ShoppingCart.ProductsAndAmount.First().Id, product.Id);
        }

        [Test]
        public async Task Update_WithWrongCustomerId_WillReturnNotFound()
        {
            var body = JsonSerializer.Serialize(
                new UpdateShoppingCartRequest { ProductId = Guid.NewGuid() });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var result = await this.httpClient.PutAndEnsureNotFound(UpdateShoppingCartRequest.BuildRoute(Guid.NewGuid()), content);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }
    }
}