using System.Collections.Generic;
using System.Threading.Tasks;

using Ordering.Domain.Interfaces;

namespace Ordering.Domain.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllForCustomer(ISpecification<Order> specification);
        Task<Order> GetForCustomerId(ISpecification<Order> specification);
        Task AddAsync(Order order);
        Task<Order> UpdateOrder(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
