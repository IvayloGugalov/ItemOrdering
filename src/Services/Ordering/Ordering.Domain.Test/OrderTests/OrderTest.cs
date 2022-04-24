using System;
using System.Collections.Generic;

using GuidGenerator;
using NUnit.Framework;

using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Shared;

namespace Ordering.Domain.Test.OrderTests
{
    [TestFixture]
    public class OrderTest
    {
        private readonly IGuidGeneratorService guidGenerator = new GuidGeneratorService();

        [Test]
        [TestCaseSource(nameof(OrderTest.emptyList))]
        public void OrderWithNoProducts_CantBeCreated_WillThrowException(List<OrderedProduct> products)
        {
            Assert.Throws<ArgumentNullException>(() => new Order(Guid.NewGuid(), products, this.guidGenerator));
        }

        [Test]
        public void SetShippingAddress_WillChangeAddress_Successfully()
        {
            var address = new Address(
                country: "Bulgaria",
                city: "Sofia",
                zipCode: 1000,
                street: "4-ti Kilometyr",
                streetNumber: 1);
            var products = new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 25.5, 2),
                new OrderedProduct(Guid.NewGuid(), 44.3, 10)
            };
            var order = new Order(Guid.NewGuid(), products, this.guidGenerator);

            Assert.IsNull(order.ShippingAddress);

            order.SetShippingAddress(address);

            Assert.AreEqual(address, order.ShippingAddress);
        }

        [Test]
        public void CalculateTotalPrice_WillReturnValue_Successfully()
        {
            var products = new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 25.5, 2),
                new OrderedProduct(Guid.NewGuid(), 44.3, 10)
            };
            var order = new Order(Guid.NewGuid(), products, this.guidGenerator);

            var totalPrice = 0.0;
            foreach (var product in products)
            {
                for (var i = 0; i < product.Amount; i++)
                {
                    totalPrice += product.Price;
                }
            }

            Assert.AreEqual(Math.Round(totalPrice, 2), order.CalculateTotalPrice());
        }

        private static object[] emptyList =
        {
            new List<OrderedProduct>(),
            null
        };
    }
}
