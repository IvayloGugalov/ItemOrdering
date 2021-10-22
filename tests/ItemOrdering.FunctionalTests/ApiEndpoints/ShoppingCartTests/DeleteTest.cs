using System;
using System.Linq;
using System.Net;
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
    public class DeleteTest
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
        public async Task Delete_WillSucceed()
        {
            using var scope = this.app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ItemOrderingDbContext>();

            var customer = dbContext.Customers.FirstOrDefault();
            if (customer is null) throw new ArgumentNullException(nameof(customer), "No customer created during seed operation.");

            var result = await this.httpClient.DeleteAndEnsureNoContent(DeleteShoppingCartRequest.BuildRoute(customer.Id));

            Assert.IsTrue(result.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public async Task Delete_WithWrongCustomerId_WillReturnNotFound()
        {
            var result = await this.httpClient.DeleteAndEnsureNotFound(DeleteShoppingCartRequest.BuildRoute(Guid.NewGuid()));

            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
