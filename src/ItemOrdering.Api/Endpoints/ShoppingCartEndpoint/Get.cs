using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class Get : BaseAsyncEndpoint
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public Get(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> AddProductToShoppingCartAsync(Guid id)
        {
            var shoppingCart = await this.shoppingCartRepository.GetShoppingCartWithProducts(id);

            return Ok(shoppingCart);
        }
    }
}
