using System.Collections.Generic;

namespace ItemOrdering.Api.Endpoints.OrderEndpoint
{
    public class GetOrderResponse
    {
        public OrderDto OrderDto { get; set; }
    }

    public class GetOrdersResponse
    {
        public List<OrderDto> OrdersDto { get; set; }
    }
}
