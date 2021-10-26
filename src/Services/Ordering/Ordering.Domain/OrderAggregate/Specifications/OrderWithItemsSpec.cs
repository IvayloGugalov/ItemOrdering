using System;
using System.Linq;

using Ordering.Domain.Shared;

namespace Ordering.Domain.OrderAggregate.Specifications
{
    public static class OrderWithItems
    {
        public static IQueryable<Order> GetProductsForOrder(this IQueryable<Order> query, Guid id)
        {
            return query.Specify(new OrderWithItemsSpec(id));
        }

        private class OrderWithItemsSpec : BaseSpecification<Order>
        {
            public OrderWithItemsSpec(Guid customerId)
            {
                this.Criteria = x => x.CustomerId == customerId;
                this.Includes.Add(x => x.OrderedProducts);
            }
        }
    }
}
