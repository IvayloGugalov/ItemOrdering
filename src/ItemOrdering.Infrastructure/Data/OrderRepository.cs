using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ItemOrderingDbContext context;

        public OrderRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        public Task<IEnumerable<Order>> GetAllForCustomerAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var result = await this.context.Orders.AddAsync(order);
            await this.context.SaveChangesAsync();

            return result.Entity;
        }

        public Task<Order> UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Task RemoveOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
