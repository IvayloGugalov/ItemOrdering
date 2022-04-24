using System;
using System.Collections.Generic;
using System.Linq;

using GuidGenerator;
using NUnit.Framework;

using Ordering.Domain.Exceptions;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.ShoppingCartAggregate;
using Ordering.Domain.Test.EntityBuilders;

namespace Ordering.Domain.Test.CustomerTests
{
    [TestFixture]
    public class CustomerTest
    {
        private readonly IGuidGeneratorService guidGenerator = new GuidGeneratorService();

        [Test]
        public void CreateShoppingCart_WhileCartExists_WillThrowException()
        {
            var customer = CustomerBuilder.CreateCustomer(this.guidGenerator);
            var shoppingCart = new ShoppingCart(customer.Id, this.guidGenerator);

            customer.SetShoppingCart(shoppingCart.Id);

            Assert.Throws<ShoppingCartMappedException>(() => customer.SetShoppingCart(shoppingCart.Id));
        }

        [Test]
        public void AddOrder_OnAddingDuplicateOrder_WillNotAdd()
        {
            var customer = CustomerBuilder.CreateCustomer(this.guidGenerator);
            var order = new Order(customer.Id, new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 100, 1)
            }, this.guidGenerator);

            customer.AddOrder(order);
            customer.AddOrder(order);

            Assert.AreEqual(1, customer.Orders.Count);
            Assert.AreEqual(order, customer.Orders.First());
        }

        [Test]
        public void RemoveOrder_OnNonExistentOrder_WillNotReturnFalse()
        {
            var customer = CustomerBuilder.CreateCustomer(this.guidGenerator);
            var order = new Order(customer.Id, new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 100, 1)
            }, this.guidGenerator);
            var notAddedOrder = new Order(Guid.NewGuid(), new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 9999.99, 10)
            }, this.guidGenerator);

            customer.AddOrder(order);

            Assert.AreEqual(1, customer.Orders.Count);

            var result = customer.RemoveOrder(notAddedOrder);

            Assert.IsFalse(result);
        }
    }
}
