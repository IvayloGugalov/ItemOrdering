using System;
using System.Threading.Tasks;

using GuardClauses;

using Ordering.Domain.Interfaces;
using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;
using Ordering.Domain.ShoppingCartAggregate.Specifications;

namespace Ordering.Domain.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        public async Task<ShoppingCart> GetOrCreateShoppingCartAsync(Guid customerId)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            var spec = new ShoppingCartWithProductsSpec(customerId);
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerAsync(spec);

            if (shoppingCart != null) return shoppingCart;

            shoppingCart = new ShoppingCart(customerId);

            await this.shoppingCartRepository.AddAsync(shoppingCart);

            return shoppingCart;
        }

        public async Task<bool> AddProductToShoppingCartAsync(ShoppingCart shoppingCart, Guid productId)
        {
            Guard.Against.NullOrEmpty(productId, nameof(productId));
            var product = await this.productRepository.GetByIdAsync(productId);

            if (product == null) return false;

            shoppingCart.AddProduct(product);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid customerId)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            var spec = new ShoppingCartWithProductsSpec(customerId);
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerAsync(spec);

            if (shoppingCart is null) return false;

            await this.shoppingCartRepository.DeleteAsync(shoppingCart);
            return true;
        }
    }
}
