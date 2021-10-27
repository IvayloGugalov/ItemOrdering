using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public Create(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpPost(CreateShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<CreateShoppingCartResponse>> CreateShoppingCartAsync([FromRoute]CreateShoppingCartRequest request)
        {
            // TODO: Should we check for customer id?
            var shoppingCart = await this.shoppingCartService.GetOrCreateShoppingCartAsync(request.CustomerId);

            var result = new CreateShoppingCartResponse { Id = shoppingCart.Id};

            return Ok(result);
        }
    }
}
