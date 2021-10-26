using System;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    public record ProductAndAmountDTO(Guid Id, double Price, int Amount);
}