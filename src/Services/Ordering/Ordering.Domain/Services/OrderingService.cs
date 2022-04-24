using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GuardClauses;
using GuidGenerator;

using Ordering.Domain.Exceptions;
using Ordering.Domain.Interfaces;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.OrderAggregate.Specifications;
using Ordering.Domain.ShoppingCartAggregate;
using Ordering.Domain.ShoppingCartAggregate.Specifications;

namespace Ordering.Domain.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IGuidGeneratorService guidGenerator;

        public OrderingService(
            IShoppingCartRepository shoppingCartRepository,
            IOrderRepository orderRepository,
            IGuidGeneratorService guidGenerator)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.orderRepository = orderRepository;
            this.guidGenerator = guidGenerator;
        }

        public async Task<Order> CreateOrderFromShoppingCart(Guid customerId)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            var spec = new ShoppingCartWithProductsSpec(customerId);
            var shoppingCart = await this.shoppingCartRepository.FindByCustomerAsync(spec);

            if (shoppingCart == null) throw new InvalidShoppingCartForCustomerException("Given customer doesn't have any shopping cart");
            if (!shoppingCart.ProductsAndAmount.Any()) throw new EmptyBasketOnCheckoutException();

            var order = CreateOrderFromShoppingCart(shoppingCart);
            await this.orderRepository.AddAsync(order);

            shoppingCart.Clear();
            await this.shoppingCartRepository.UpdateAsync(shoppingCart);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForCustomerAsync(Guid customerId)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            var specification = new OrdersSortedByDateSpec(customerId);
            return await this.orderRepository.GetAllForCustomer(specification);
        }

        public async Task<Order> GetOrderForCustomerAsync(Guid customerId)
        {
            Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            var specification = new OrderWithProductsSpec(customerId);
            return await this.orderRepository.GetForCustomerId(specification);
        }

        private Order CreateOrderFromShoppingCart(ShoppingCart shoppingCart)
        {
            var orderedProducts = new List<OrderedProduct>();
            foreach (var productAndAmount in shoppingCart.ProductsAndAmount)
            {
                orderedProducts.Add(new OrderedProduct(productAndAmount.ProductId, productAndAmount.Price, productAndAmount.Amount));
                // TODO: Publish event
            }
            return new Order(shoppingCart.CustomerId, orderedProducts, this.guidGenerator);
        }
    }
}
