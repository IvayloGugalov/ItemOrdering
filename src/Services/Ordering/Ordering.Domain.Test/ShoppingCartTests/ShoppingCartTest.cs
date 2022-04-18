using System;
using System.Linq;

using GuidGenerator;
using NUnit.Framework;

using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.Test.ShoppingCartTests
{
    [TestFixture]
    public class ShoppingCartTest
    {
        private IGuidGeneratorService guidGenerator;

        [SetUp]
        public void SetUp()
        {
            this.guidGenerator = new GuidGeneratorService();
        }

        [Test]
        public void AddProduct_WhenPassingNull_WillThrowException()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);

            Assert.Throws<ArgumentNullException>(() => shoppingCart.AddProduct(null));
        }

        [Test]
        public void AddProduct_WhenProductExistsInShoppingCart_WillIncreaseTheAmount()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);
            var shop = new Shop("www.shop.url.com", "my shop", this.guidGenerator);
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop, this.guidGenerator);

            shoppingCart.AddProduct(product);
            shoppingCart.AddProduct(product);

            Assert.AreEqual(1, shoppingCart.ProductsAndAmount.Count);
            Assert.AreEqual(product.Id, shoppingCart.ProductsAndAmount.First().ProductId);
            Assert.AreEqual(2, shoppingCart.ProductsAndAmount.First().Amount);
        }

        [Test]
        public void RemoveProduct_OnExistingProduct_WillBeRemoved()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid(), this.guidGenerator);
            var shop = new Shop("www.shop.url.com", "my shop", this.guidGenerator);
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop, this.guidGenerator);

            shoppingCart.AddProduct(product);

            Assert.AreEqual(product.Id, shoppingCart.ProductsAndAmount.First().ProductId);

            var result = shoppingCart.RemoveProduct(shoppingCart.ProductsAndAmount.First());

            Assert.IsTrue(result);
            Assert.AreEqual(0, shoppingCart.ProductsAndAmount.Count);
        }
    }
}
