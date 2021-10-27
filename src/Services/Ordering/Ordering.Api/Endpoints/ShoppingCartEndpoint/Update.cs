using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Update : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public Update(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpPut(UpdateShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<UpdateShoppingCartResponse>> AddProductToShoppingCartAsync(
            [FromRoute]Guid customerId,
            [FromBody]UpdateShoppingCartRequest request)
        {
            var shoppingCart = await this.shoppingCartService.GetOrCreateShoppingCartAsync(customerId);

            var isAdded = await this.shoppingCartService.AddProductToShoppingCartAsync(shoppingCart, request.ProductId);

            if (!isAdded) return NotFound(new ErrorResponse("No such product."));

            var result = new UpdateShoppingCartResponse
            {
                ShoppingCart = new ShoppingCartDto(
                    shoppingCart.Id,
                    shoppingCart.ProductsAndAmount.MapProductsAndAmountToDTO())
            };

            return Ok(result);
        }
    }
}
