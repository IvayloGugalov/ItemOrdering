﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using ItemOrdering.Domain.OrderAggregate.Specifications;
using ItemOrdering.FunctionalTests.HttpExtension;
using ItemOrdering.Infrastructure.Data;
using ItemOrdering.Web;
using ItemOrdering.Web.Endpoints.OrderEndpoint;
using ItemOrdering.Web.Endpoints.ShoppingCartEndpoint;

namespace ItemOrdering.FunctionalTests.ApiEndpoints.OrderTests
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

            var customer = Seeder.CustomerWithCartAndProductsInside(dbContext);

            var result = await this.httpClient.PostAndReceiveMessage(CreateOrderRequest.BuildRoute(customer.Id));

            var order = await dbContext.Orders.GetProductsForOrder(customer.Id).SingleOrDefaultAsync();
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.AreEqual(customer.Id, order.CustomerId);
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