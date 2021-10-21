using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemOrdering.Domain.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllByCustomerIdAsync(Guid customerId);
        Task<Order> GetByCustomerIdWithProductsAsync(Guid customerId);
        Task AddAsync(Order order);
        Task<Order> UpdateOrder(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
