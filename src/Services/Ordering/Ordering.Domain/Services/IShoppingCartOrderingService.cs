using System;
using System.Threading.Tasks;

using Ordering.Domain.OrderAggregate;

namespace Ordering.Domain.Services
{
    public interface IShoppingCartOrderingService
    {
        Task<Order> CreateOrderFromShoppingCart(Guid customerId);
    }
}