using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.API.Endpoints.ShoppingCartEndpoint
{
    [ApiController]
    public class Update : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public Update(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpPut(UpdateShoppingCartRequest.ROUTE)]
        public async Task<ActionResult<UpdateShoppingCartResponse>> AddProductToShoppingCartAsync(
            [FromRoute]Guid customerId,
            [FromBody]UpdateShoppingCartRequest request)
        {
            // TODO: Should we check for existing Customer Id?
            var product = await this.productRepository.GetByIdAsync(request.ProductId);

            if (product == null) return NotFound(request.ProductId);

            var shoppingCart = await this.shoppingCartRepository.FindByCustomerIncludeProducts(customerId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(customerId);
                shoppingCart.AddProduct(product);

                await this.shoppingCartRepository.AddAsync(shoppingCart);
            }
            else
            {
                shoppingCart.AddProduct(product);
                await this.shoppingCartRepository.UpdateAsync(shoppingCart);
            }

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
