using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Interfaces
{
    public interface IOrderingService
    {
        Task<Order> CreateOrderFromShoppingCart(Guid customerId);
        Task<Order> GetOrderForCustomerAsync(Guid customerId);
        Task<IEnumerable<Order>> GetOrdersForCustomerAsync(Guid customerId);
    }
}