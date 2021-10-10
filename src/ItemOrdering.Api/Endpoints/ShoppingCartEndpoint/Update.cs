using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemOrdering.Domain.ShoppingCartAggregate;
using Microsoft.AspNetCore.Mvc;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
{
    public class Update : BaseAsyncEndpoint
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public Update(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpPut("shoppingcart/{id:guid}")]
        public async Task<ActionResult<ShoppingCartResult>> AddProductToShoppingCartAsync(Guid id, [FromBody]Command request)
        {
            var shoppingCart = await this.shoppingCartRepository.GetShoppingCartForCustomer(id);
            var product = await this.productRepository.GetProductAsync(request.ProductId);

            shoppingCart.AddProduct(product);

            await this.shoppingCartRepository.UpdateShoppingCart(shoppingCart);

            var result = new ShoppingCartResult(shoppingCart.Id);

            return Ok(result);
        }
    }

    public class Command
    {
        public Guid ProductId{ get; set; }
    }
}
