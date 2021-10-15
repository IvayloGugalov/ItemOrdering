using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Web.Endpoints.ShoppingCartEndpoint
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
            Guid customerId,
            UpdateShoppingCartRequest request)
        {
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerIncludeProducts(customerId);

            if (shoppingCart == null) return NotFound(customerId);

            var product = await this.productRepository.GetProductByIdAsync(request.ProductId);

            if (product == null) return NotFound(request.ProductId);

            shoppingCart.AddProduct(product);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);

            var result = new UpdateShoppingCartResponse
            {
                ShoppingCart = new ShoppingCartDTO(
                    shoppingCart.Id,
                    shoppingCart.ProductsAndAmount.MapProductsAndAmountToDTO())
            };

            return Ok(result);
        }
    }
}
