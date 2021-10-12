using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Get : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Get(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShoppingCartDTO>> AddProductToShoppingCartAsync(Guid id)
        {
            var shoppingCart = await this.shoppingCartRepository.GetShoppingCartByCustomerIdAsync(id);

            if (shoppingCart is null) return NotFound();

            var shoppingCartDTO = new ShoppingCartDTO(shoppingCart.Id, shoppingCart.ProductsAndAmount.MapProductsAndAmountToDTO());

            return Ok(shoppingCartDTO);
        }
    }
}
