using System;
using System.Collections.Generic;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public record ShoppingCartDto(Guid Id, HashSet<ProductAndAmountDTO> ProductsAndAmount);
}
