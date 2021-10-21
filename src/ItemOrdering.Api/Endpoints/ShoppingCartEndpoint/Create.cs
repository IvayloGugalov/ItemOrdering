using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Create : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Create(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost(CreateShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<CreateShoppingCartResponse>> CreateShoppingCartAsync([FromRoute]CreateShoppingCartRequest request)
        {
            // TODO: Should we check for customer id?
            var shoppingCart = new ShoppingCart(request.CustomerId);

            await this.shoppingCartRepository.AddAsync(shoppingCart);

            var result = new CreateShoppingCartResponse { Id = shoppingCart.Id};

            return Ok(result);
        }
    }
}
