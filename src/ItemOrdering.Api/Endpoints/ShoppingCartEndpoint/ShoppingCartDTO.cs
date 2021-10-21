using System;
using System.Collections.Generic;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public record ShoppingCartDto(Guid Id, HashSet<ProductAndAmountDTO> ProductsAndAmount);
}
