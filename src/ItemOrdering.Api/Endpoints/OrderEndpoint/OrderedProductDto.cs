using System;

namespace ItemOrdering.Api.Endpoints.OrderEndpoint
{
    public record OrderedProductDto(Guid Id, double Price, int Amount);
}
