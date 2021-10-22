using System.Collections.Generic;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Api.Endpoints.ShoppingCartEndpoint
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