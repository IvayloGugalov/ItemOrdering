using System;

using Ordering.Domain.Shared;

namespace Ordering.Domain.ShoppingCartAggregate.Specifications
{
    public class ShoppingCartWithProductsSpec : BaseSpecification<ShoppingCart>
    {
        public ShoppingCartWithProductsSpec(Guid customerId)
        {
            this.Criteria = x => x.CustomerId == customerId;
            this.Includes.Add(x => x.ProductsAndAmount);
        }
    }
}
