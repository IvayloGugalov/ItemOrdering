using System;

using Ordering.Domain.Shared;

namespace Ordering.Domain.OrderAggregate.Specifications
{
    public class OrderWithProductsSpec : BaseSpecification<Order>
    {
        public OrderWithProductsSpec(Guid customerId)
        {
            this.Criteria = x => x.CustomerId == customerId;
            this.Includes.Add(x => x.OrderedProducts);
        }
    }
}
