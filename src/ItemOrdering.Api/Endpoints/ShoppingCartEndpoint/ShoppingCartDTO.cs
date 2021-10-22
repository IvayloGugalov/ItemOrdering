using System;
using System.Collections.Generic;

namespace ItemOrdering.Api.Endpoints.ShoppingCartEndpoint
{
    public record ShoppingCartDto(Guid Id, HashSet<ProductAndAmountDTO> ProductsAndAmount);
}
