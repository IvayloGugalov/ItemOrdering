using System;

namespace Ordering.API.Endpoints.OrderEndpoint
{
    public record OrderedProductDto(Guid Id, double Price, int Amount);
}
