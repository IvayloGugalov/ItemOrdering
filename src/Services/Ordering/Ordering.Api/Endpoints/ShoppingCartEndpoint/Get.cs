using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Get(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet(GetShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<GetShoppingCartResponse>> AddProductToShoppingCartAsync([FromRoute]GetShoppingCartRequest request)
        {
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerIncludeProducts(request.CustomerId);

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
