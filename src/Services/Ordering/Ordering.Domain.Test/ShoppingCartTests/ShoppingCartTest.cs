using System;
using System.Linq;

using NUnit.Framework;

using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.Test.ShoppingCartTests
{
    [TestFixture]
    public class ShoppingCartTest
    {
        [Test]
        public void AddProduct_WhenPassingNull_WillThrowException()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid());

            Assert.Throws<ArgumentNullException>(() => shoppingCart.AddProduct(null));
        }

        [Test]
        public void AddProduct_WhenProductExistsInShoppingCart_WillIncreaseTheAmount()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid());
            var shop = new Shop("www.shop.url.com", "my shop");
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop);

            shoppingCart.AddProduct(product);
            shoppingCart.AddProduct(product);

            Assert.AreEqual(1, shoppingCart.ProductsAndAmount.Count);
            Assert.AreEqual(product.Id, shoppingCart.ProductsAndAmount.First().ProductId);
            Assert.AreEqual(2, shoppingCart.ProductsAndAmount.First().Amount);
        }

        [Test]
        public void RemoveProduct_OnExistingProduct_WillBeRemoved()
        {
            var shoppingCart = new ShoppingCart(Guid.NewGuid());
            var shop = new Shop("www.shop.url.com", "my shop");
            var product = new Product(
                url: @"www.shop.url.com/product_1", title: "product_1", description: "This is the description", price: 49.99, shop);

            shoppingCart.AddProduct(product);

            Assert.AreEqual(product.Id, shoppingCart.ProductsAndAmount.First().ProductId);

            var result = shoppingCart.RemoveProduct(shoppingCart.ProductsAndAmount.First());

            Assert.IsTrue(result);
            Assert.AreEqual(0, shoppingCart.ProductsAndAmount.Count);
        }
    }
}
