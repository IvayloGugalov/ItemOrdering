using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Services;

namespace Ordering.API.Endpoints.OrderEndpoint
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly IShoppingCartOrderingService shoppingCartOrderingService;

        public Create(IShoppingCartOrderingService shoppingCartOrderingService)
        {
            this.shoppingCartOrderingService = shoppingCartOrderingService;
        }

        [HttpPost(CreateOrderRequest.ROUTE)]
        public async Task<ActionResult> CreateOrderAsync([FromRoute]CreateOrderRequest request)
        {
           await this.shoppingCartOrderingService.CreateOrderFromShoppingCart(request.CustomerId);

           return NoContent();
        }
    }
}
