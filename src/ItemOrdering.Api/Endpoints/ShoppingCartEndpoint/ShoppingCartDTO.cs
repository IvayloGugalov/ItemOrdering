using System;
using System.Collections.Generic;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public record ShoppingCartDTO(Guid Id, IReadOnlySet<ProductAndAmountDTO> ProductsAndAmount);
}
