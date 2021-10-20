using System;
using System.Collections.Generic;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    public record OrderDto(Guid Id, Address ShippingAddress, DateTime Created, List<OrderedProductDto> OrderedProducts);
}
