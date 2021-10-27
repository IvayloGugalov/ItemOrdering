using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public Get(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet(GetShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<GetShoppingCartResponse>> GetToShoppingCartAsync([FromRoute]GetShoppingCartRequest request)
        {
            var shoppingCart = await this.shoppingCartService.GetOrCreateShoppingCartAsync(request.CustomerId);

            if (shoppingCart is null) return NotFound();

            var result = new GetShoppingCartResponse
            {
                ShoppingCartDto = new ShoppingCartDto(
                    shoppingCart.Id, shoppingCart.ProductsAndAmount.MapProductsAndAmountToDTO())
            };

            return Ok(result);
        }
    }
}
