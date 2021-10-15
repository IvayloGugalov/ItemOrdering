using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.OrderAggregate.Specifications;

namespace ItemOrdering.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ItemOrderingDbContext context;

        public OrderRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        public Task<IEnumerable<Order>> GetAllByIdWithProductsAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetByIdWithProductsAsync(Guid customerId)
        {
            return await this.context.Orders.GetProductsForOrder(customerId)
                .SingleOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<Order> AddAsync(Order order)
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
