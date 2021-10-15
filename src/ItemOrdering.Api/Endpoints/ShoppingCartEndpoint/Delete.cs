using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Delete : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Delete(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpDelete(UpdateShoppingCartRequest.ROUTE)]
        public async Task<ActionResult> AddProductToShoppingCartAsync(Guid customerId)
        {
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerIncludeProducts(customerId);

            if (shoppingCart == null) return NotFound(customerId);

            await this.shoppingCartRepository.DeleteAsync(shoppingCart);

            return NoContent();
        }
    }
}
