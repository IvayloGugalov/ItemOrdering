using System;
using System.Linq;

using Ordering.Domain.Shared;

namespace Ordering.Domain.ShoppingCartAggregate.Specifications
{
    public static class ShoppingCartWithProducts
    {
        public static IQueryable<ShoppingCart> GetProductsForCart(this IQueryable<ShoppingCart> query, Guid id)
        {
            return query.Specify(new ShoppingCartWithProductsSpec(id));
        }

        private class ShoppingCartWithProductsSpec : BaseSpecification<ShoppingCart>
        {
            public ShoppingCartWithProductsSpec(Guid customerId)
            {
                this.Criteria = x => x.CustomerId == customerId;
                this.Includes.Add(x => x.ProductsAndAmount);
            }
        }
    }
}
