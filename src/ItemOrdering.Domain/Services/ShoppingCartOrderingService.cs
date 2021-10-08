using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Domain.Services
{
    public class ShoppingCartOrderingService : IShoppingCartOrderingService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IOrderRepository orderRepository;

        public ShoppingCartOrderingService(IShoppingCartRepository shoppingCartRepository, IOrderRepository orderRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.orderRepository = orderRepository;
        }

        public async Task CreateOrderFromShoppingCart(Guid customerId)
        {
            var shoppingCart = await this.shoppingCartRepository.GetShoppingCartForCustomer(customerId);

            if (shoppingCart == null)
            {
                throw new ValidationException("Given customer doesn't have any shopping cart");
            }

            var order = CreateOrderFromShoppingCart(shoppingCart);
            await this.orderRepository.CreateOrderAsync(order);

            shoppingCart.Clear();
        }

        private static Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart)
        {
            var orderedProducts = new List<OrderedProduct>();
            foreach (var (product, amount) in shoppingCart.ProductsAndAmount)
            {
                orderedProducts.Add(new OrderedProduct(product.Id, product.OriginalPrice.Value, amount));
                // TODO: Publish event
            }
            return Order.CreateOrder(shoppingCart.CustomerId, orderedProducts);
        }
    }
}
