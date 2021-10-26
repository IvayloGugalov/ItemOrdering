using System.Collections.Generic;

using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public static class Mappers
    {
        public static HashSet<ProductAndAmountDTO> MapProductsAndAmountToDTO(this IReadOnlySet<ProductAndAmount> products)
        {
            var returnedSet = new HashSet<ProductAndAmountDTO>();

            foreach (var product in products)
            {
                returnedSet.Add(new ProductAndAmountDTO(product.ProductId, product.Price, product.Amount));
            }

            return returnedSet;
        }
    }
}