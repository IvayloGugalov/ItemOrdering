using System;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using Ordering.Domain.Exceptions;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Services;
using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.Test.ServicesTests
{
    [TestFixture]
    public class ShoppingCartOrderingServiceTest
    {
        private Mock<IShoppingCartRepository> shoppingCartRepositoryMock;
        private Mock<IOrderRepository> orderRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            this.shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
            this.orderRepositoryMock = new Mock<IOrderRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            this.shoppingCartRepositoryMock.VerifyAll();
            this.orderRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task CreateOrderFromShoppingCart_WithValidParams_WillCreateShoppingCart()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid());
            var shop = new Shop("www.shop.url.com", "my shop");
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop);

            shoppingCart.AddProduct(product);

            var shoppingCartOrderingService = new ShoppingCartOrderingService(
                this.shoppingCartRepositoryMock.Object,
                this.orderRepositoryMock.Object);

            this.shoppingCartRepositoryMock.Setup(_ => _.FindByCustomerIncludeProducts(shoppingCart.CustomerId))
                .ReturnsAsync(shoppingCart);

            this.orderRepositoryMock.Setup(_ => _.AddAsync(It.IsAny<Order>()));

            await shoppingCartOrderingService.CreateOrderFromShoppingCart(shoppingCart.CustomerId);

            this.orderRepositoryMock.Verify(_ => _.AddAsync(It.IsAny<Order>()), Times.Once);
            Assert.AreEqual(0, shoppingCart.ProductsAndAmount.Count);
        }

        [Test]
        public void CreateOrderFromShoppingCart_WithInvalidShoppingCart_WillThrow()
        {
            var customerId = Guid.NewGuid();
            var shoppingCartOrderingService = new ShoppingCartOrderingService(
                this.shoppingCartRepositoryMock.Object,
                this.orderRepositoryMock.Object);

            this.shoppingCartRepositoryMock.Setup(_ => _.FindByCustomerIncludeProducts(customerId))
                .ReturnsAsync(It.IsAny<ShoppingCart>());

            Assert.ThrowsAsync<InvalidShoppingCartForCustomerException>(async () => await shoppingCartOrderingService.CreateOrderFromShoppingCart(customerId));
        }
    }
}
