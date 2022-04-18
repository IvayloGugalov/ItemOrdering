using System;
using System.Linq;
using System.Threading.Tasks;

using GuidGenerator;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

using Ordering.Domain.ShoppingCartAggregate;
using Ordering.Domain.ShoppingCartAggregate.Specifications;
using Ordering.Domain.Test.EntityBuilders;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Test.UnitTests
{
    [TestFixture]
    public class ShoppingCartRepositoryTest
    {
        private ItemOrderingDbContext dbContext;
        private ShoppingCartRepository shoppingCartRepository;
        private IGuidGeneratorService guidGenerator;

        [SetUp]
        public void SetUp()
        {
            var dbOptions = new DbContextOptionsBuilder<ItemOrderingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestOrdering")
                .Options;
            this.dbContext = new ItemOrderingDbContext(dbOptions);
            this.shoppingCartRepository = new ShoppingCartRepository(this.dbContext);
            this.guidGenerator = new GuidGeneratorService();
        }

        [Test]
        public async Task AddAsync_WillSucceed()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);

            await this.shoppingCartRepository.AddAsync(shoppingCart);

            var cartFromDb = this.dbContext.ShoppingCarts.FirstOrDefault(x => x.Id == shoppingCart.Id);

            Assert.AreEqual(shoppingCart, cartFromDb);
        }

        [Test]
        public async Task UpdateAsync_WillSucceed()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);
            var product = ProductBuilder.CreateProduct(this.guidGenerator);

            shoppingCart.AddProduct(product);
            this.dbContext.SeedDataBaseWith(shoppingCart);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);

            var cartFromDb = this.dbContext.ShoppingCarts.FirstOrDefault(x => x.Id == shoppingCart.Id);

            Assert.AreEqual(shoppingCart, cartFromDb);
        }

        [Test]
        public async Task FindByCustomerIncludeProducts_WillIncludeProducts()
        {
            var customerId = Guid.NewGuid();
            var shoppingCart = new ShoppingCart(customerId, this.guidGenerator);
            var product = ProductBuilder.CreateProduct(this.guidGenerator);

            shoppingCart.AddProduct(product);
            this.dbContext.SeedDataBaseWith(shoppingCart);

            var cartFromDb = await this.shoppingCartRepository.FindByCustomerAsync(new ShoppingCartWithProductsSpec(customerId));

            Assert.AreEqual(shoppingCart, cartFromDb);
        }

        [Test]
        public async Task DeleteAsync_WillSucceed()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);
            var product = ProductBuilder.CreateProduct(this.guidGenerator);

            shoppingCart.AddProduct(product);
            this.dbContext.SeedDataBaseWith(shoppingCart);

            await this.shoppingCartRepository.DeleteAsync(shoppingCart);

            var cartFromDb = this.dbContext.ShoppingCarts.FirstOrDefault(x => x.Id == shoppingCart.Id);

            Assert.IsNull(cartFromDb);
        }
    }
}
