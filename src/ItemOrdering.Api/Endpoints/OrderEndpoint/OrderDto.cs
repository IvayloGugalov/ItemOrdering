using System;
using System.Collections.Generic;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Api.Endpoints.OrderEndpoint
{
    public record OrderDto(Guid Id, Address ShippingAddress, DateTime Created, List<OrderedProductDto> OrderedProducts);
}
