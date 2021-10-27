using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.OrderEndpoint
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly IOrderingService orderingService;

        public Create(IOrderingService orderingService)
        {
            this.orderingService = orderingService;
        }

        [HttpPost(CreateOrderRequest.ROUTE)]
        public async Task<ActionResult> CreateOrderAsync([FromRoute]CreateOrderRequest request)
        {
           await this.orderingService.CreateOrderFromShoppingCart(request.CustomerId);

           return NoContent();
        }
    }
}
