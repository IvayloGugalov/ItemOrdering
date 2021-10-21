using System;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public record OrderedProductDto(Guid Id, double Price, int Amount);
}
