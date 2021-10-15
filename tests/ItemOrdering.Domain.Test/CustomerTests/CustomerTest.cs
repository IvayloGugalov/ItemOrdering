using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.Domain.Exceptions;
using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.Shared;
using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Domain.Test.CustomerTests
{
    [TestFixture]
    public class CustomerTest
    {
        [Test]
        public void CreateShoppingCart_WhileCartExists_WillThrowException()
        {
            var customer = new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"));
            var shoppingCart = new ShoppingCart(customer.Id);

            customer.SetShoppingCart(shoppingCart.Id);

            Assert.Throws<ShoppingCartMappedException>(() => customer.SetShoppingCart(shoppingCart.Id));
        }

        [Test]
        public void AddOrder_OnAddingDuplicateOrder_WillNotAdd()
        {
            var customer = new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"));
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
            var customer = new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"));
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
