using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllByIdWithProductsAsync(Guid customerId);
        Task<Order> GetByIdWithProductsAsync(Guid orderId);
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateOrder(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
