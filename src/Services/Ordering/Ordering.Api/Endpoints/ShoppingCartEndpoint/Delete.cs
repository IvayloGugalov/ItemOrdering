using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Interfaces;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Delete : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;

        public Delete(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        [HttpDelete(DeleteShoppingCartRequest.ROUTE)]
        public async Task<ActionResult> DeleteShoppingCartAsync([FromRoute]DeleteShoppingCartRequest request)
        {
            var result = await this.shoppingCartService.DeleteAsync(request.CustomerId);

            return result ? NoContent() : NotFound();
        }
    }
}
