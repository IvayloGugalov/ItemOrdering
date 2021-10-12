using System.Collections.Generic;
using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public static class Mappers
    {
        public static IReadOnlySet<ProductAndAmountDTO> MapProductsAndAmountToDTO(this IReadOnlySet<ProductAndAmount> products)
        {
            var returnedSet = new HashSet<ProductAndAmountDTO>();

            foreach (var product in products)
            {
                returnedSet.Add(new ProductAndAmountDTO(product.Price, product.Amount));
            }

            return returnedSet;
        }
    }
}