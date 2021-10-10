using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class Create : BaseAsyncEndpoint
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Create(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost("{id:guid}")]
        public async Task<ActionResult<ShoppingCartResult>> CreateShoppingCartAsync(Guid id)
        {
            var shoppingCart = new ShoppingCart(id);

            await this.shoppingCartRepository.AddShoppingCart(shoppingCart);

            var result = new ShoppingCartResult(shoppingCart.Id);

            return Ok(result);
        }
    }
}
