using System;
using System.Collections.Generic;

using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Test.EntityBuilders
{
    public static class OrderBuilder
    {
        public static Order CreateOrder()
        {
            var products = new List<OrderedProduct>
            {
                new OrderedProduct(Guid.NewGuid(), 55.55, 2),
                new OrderedProduct(Guid.NewGuid(), 5589.55, 1)
            };

            return new Order(
                customerId: Guid.NewGuid(),
                orderedProducts: products);
        }

        public static Order CreateSpecificOrder(
            Guid customerId,
            List<OrderedProduct> orderedProducts)
        {
            return new Order(
                customerId: customerId,
                orderedProducts: orderedProducts);
        }

        public static IEnumerable<Order> CreateOrders(Guid customerId, int range)
        {
            for (var i = 1; i <= range; i++)
            {
                yield return CreateSpecificOrder(
                    customerId: customerId,
                    orderedProducts: new List<OrderedProduct>
                    {
                        new OrderedProduct(Guid.NewGuid(), 1.01 * i, 1 * i),
                        new OrderedProduct(Guid.NewGuid(), 2.01 * i, 2 * i)
                    });
            }
        }
    }
}
