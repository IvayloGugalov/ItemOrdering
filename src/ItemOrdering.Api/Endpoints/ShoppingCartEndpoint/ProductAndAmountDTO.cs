using System;

namespace ItemOrdering.Api.Endpoints.ShoppingCartEndpoint
{
    public record ProductAndAmountDTO(Guid Id, double Price, int Amount);
}