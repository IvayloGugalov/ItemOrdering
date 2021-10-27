using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.OrderEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IOrderingService orderingService;

        public Get(IOrderingService orderingService)
        {
            this.orderingService = orderingService;
        }

        [HttpGet("orders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetOrders()
        {
            var id = HttpContext.User.FindFirstValue("id");

            return Ok(new List<OrderDto>
            {
                new OrderDto(Guid.NewGuid(), null, DateTime.Now, new List<OrderedProductDto>
                {
                    new OrderedProductDto(Guid.NewGuid(), 55.12, 10),
                    new OrderedProductDto(Guid.NewGuid(), 1245.12, 1021),
                    new OrderedProductDto(Guid.NewGuid(), 5425.12, 102),
                })
            });
        }

        [HttpGet(GetOrdersRequest.ROUTE)]
        public async Task<ActionResult<GetOrdersResponse>> GetAllOrdersForCustomer([FromRoute]GetOrdersRequest request)
        {
            var orders = await this.orderingService.GetOrdersForCustomerAsync(request.CustomerId);
            if (orders == null || !orders.Any()) return NoContent();

            var response = new GetOrdersResponse()
            {
                OrdersDto = orders.MapToOrdersDto()
            };

            return Ok(response);
        }


        [HttpGet(GetOrderRequest.ROUTE)]
        [Authorize]
        public async Task<ActionResult<GetOrderResponse>> GetLatestOrderForCustomer([FromRoute]GetOrderRequest request)
        {
            var order = await this.orderingService.GetOrderForCustomerAsync(request.CustomerId);

            if (order == null) return NoContent();

            var response = new GetOrderResponse()
            {
                OrderDto = order.MapToOrderDto()
            };

            return Ok(response);
        }
    }
}
