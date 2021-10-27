using System;

using Ordering.Domain.Shared;

namespace Ordering.Domain.OrderAggregate.Specifications
{
    public class OrdersSortedByDateSpec : BaseSpecification<Order>
    {
        public OrdersSortedByDateSpec(Guid customerId)
        {
            this.Criteria = x => x.CustomerId == customerId;
            this.OrderBy = x => x.Created;
        }
    }
}
