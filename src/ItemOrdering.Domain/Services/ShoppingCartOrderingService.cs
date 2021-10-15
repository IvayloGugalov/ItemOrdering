using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ItemOrdering.Domain.Exceptions;
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
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerIncludeProducts(customerId);

            if (shoppingCart == null)
            {
                throw new InvalidShoppingCartForCustomerException("Given customer doesn't have any shopping cart");
            }

            var order = CreateOrderFromShoppingCart(shoppingCart);
            await this.orderRepository.AddAsync(order);

            shoppingCart.Clear();
        }

        private static Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart)
        {
            var orderedProducts = new List<OrderedProduct>();
            foreach (var productAndAmount in shoppingCart.ProductsAndAmount)
            {
                orderedProducts.Add(new OrderedProduct(productAndAmount.ProductId, productAndAmount.Price, productAndAmount.Amount));
                // TODO: Publish event
            }
            return new Order(shoppingCart.CustomerId, orderedProducts);
        }
    }
}
