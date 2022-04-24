using System;
using System.Threading.Tasks;
using GuidGenerator;
using Moq;
using NUnit.Framework;

using Ordering.Domain.Exceptions;
using Ordering.Domain.Interfaces;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Services;
using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.Test.ServicesTests
{
    [TestFixture]
    public class ShoppingCartOrderingServiceTest
    {
        private readonly IGuidGeneratorService guidGenerator = new GuidGeneratorService();

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
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);
            var shop = new Shop("www.shop.url.com", "my shop", this.guidGenerator);
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop, this.guidGenerator);

            shoppingCart.AddProduct(product);

            var shoppingCartOrderingService = new OrderingService(
                this.shoppingCartRepositoryMock.Object,
                this.orderRepositoryMock.Object,
                this.guidGenerator);

            this.shoppingCartRepositoryMock.Setup(_ => _.FindByCustomerAsync(It.IsAny<ISpecification<ShoppingCart>>()))
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
            var shoppingCartOrderingService = new OrderingService(
                this.shoppingCartRepositoryMock.Object,
                this.orderRepositoryMock.Object,
                this.guidGenerator);

            this.shoppingCartRepositoryMock.Setup(_ => _.FindByCustomerAsync(It.IsAny<ISpecification<ShoppingCart>>()))
                .ReturnsAsync(It.IsAny<ShoppingCart>());

            Assert.ThrowsAsync<InvalidShoppingCartForCustomerException>(async () => await shoppingCartOrderingService.CreateOrderFromShoppingCart(customerId));
        }
    }
}
