using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Delete : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Delete(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpDelete(DeleteShoppingCartRequest.ROUTE)]
        public async Task<ActionResult> DeleteShoppingCartAsync([FromRoute]DeleteShoppingCartRequest request)
        {
            var shoppingCart = await this.shoppingCartRepository.FindByCustomer(request.CustomerId);

            if (shoppingCart == null) return NotFound();

            await this.shoppingCartRepository.DeleteAsync(shoppingCart);

            return NoContent();
        }
    }
}
