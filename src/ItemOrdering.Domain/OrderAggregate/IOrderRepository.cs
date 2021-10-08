using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllForCustomerAsync(Guid customerId);
        Task<Order> GetByIdAsync(Guid orderId);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrder(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
