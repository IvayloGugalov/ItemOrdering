using System;
using System.Collections.Generic;

using Ordering.Domain.Shared;

namespace Ordering.API.Endpoints.OrderEndpoint
{
    public record OrderDto(Guid Id, Address ShippingAddress, DateTime Created, List<OrderedProductDto> OrderedProducts);
}
