using System;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public record ProductAndAmountDTO(Guid Id, double Price, int Amount);
}