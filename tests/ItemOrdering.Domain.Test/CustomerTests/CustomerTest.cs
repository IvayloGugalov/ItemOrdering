using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using ItemOrdering.Domain.Exceptions;
using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.ShoppingCartAggregate;
using ItemOrdering.Domain.Test.EntityBuilders;

namespace ItemOrdering.Domain.Test.CustomerTests
{
    [TestFixture]
    public class CustomerTest
    {
        [Test]
        public void CreateShoppingCart_WhileCartExists_WillThrowException()
        {
            var customer = CustomerBuilder.CreateCustomer();
            var shoppingCart = new ShoppingCart(customer.Id);

            customer.SetShoppingCart(shoppingCart.Id);

            Assert.Throws<ShoppingCartMappedException>(() => customer.SetShoppingCart(shoppingCart.Id));
        }

        [Test]
        public void AddOrder_OnAddingDuplicateOrder_WillNotAdd()
        {
            var customer = CustomerBuilder.CreateCustomer();
            var order = new Order(customer.Id, new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 100, 1)
            });

            customer.AddOrder(order);
            customer.AddOrder(order);

            Assert.AreEqual(1, customer.Orders.Count);
            Assert.AreEqual(order, customer.Orders.First());
        }

        [Test]
        public void RemoveOrder_OnNonExistentOrder_WillNotReturnFalse()
        {
            var customer = CustomerBuilder.CreateCustomer();
            var order = new Order(customer.Id, new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 100, 1)
            });
            var notAddedOrder = new Order(Guid.NewGuid(), new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 9999.99, 10)
            });

            customer.AddOrder(order);

            Assert.AreEqual(1, customer.Orders.Count);

            var result = customer.RemoveOrder(notAddedOrder);

            Assert.IsFalse(result);
        }
    }
}
