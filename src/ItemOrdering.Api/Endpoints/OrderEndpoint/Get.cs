using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemOrdering.Domain.OrderAggregate;
using Microsoft.AspNetCore.Mvc;

namespace ItemOrdering.Web.Endpoints.OrderEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public Get(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet(GetOrdersRequest.ROUTE)]
        public async Task<ActionResult<GetOrdersResponse>> GetAllOrdersForCustomer([FromRoute]GetOrdersRequest request)
        {
            var orders = await this.orderRepository.GetAllByCustomerIdAsync(request.CustomerId);
            if (orders == null || !orders.Any()) return NoContent();

            var response = new GetOrdersResponse()
            {
                OrdersDto = orders.MapToOrdersDto()
            };

            return Ok(response);
        }


        [HttpGet(GetOrderRequest.ROUTE)]
        public async Task<ActionResult<GetOrderResponse>> GetLatestOrderForCustomer([FromRoute]GetOrderRequest request)
        {
            var order = await this.orderRepository.GetByCustomerIdWithProductsAsync(request.CustomerId);

            if (order == null) return NoContent();

            var response = new GetOrderResponse()
            {
                OrderDto = order.MapToOrderDto()
            };

            return Ok(response);
        }
    }
}
