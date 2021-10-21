using System;
using System.Threading.Tasks;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Domain.Services
{
    public interface IShoppingCartOrderingService
    {
        Task<Order> CreateOrderFromShoppingCart(Guid customerId);
    }
}