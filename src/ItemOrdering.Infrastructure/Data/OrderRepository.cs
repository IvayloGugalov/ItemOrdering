using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Order>> GetAllByIdWithProductsAsync(Guid customerId)
        {
            return await this.context.Orders.GetProductsForOrder(customerId)
                .OrderBy(x => x.Created)
                .ToListAsync();
        }

        public async Task<Order> GetByIdWithProductsAsync(Guid customerId)
        {
            return await this.context.Orders.GetProductsForOrder(customerId)
                .OrderBy(x => x.Created)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Order order)
        {
            await this.context.Orders.AddAsync(order);
            await this.context.SaveChangesAsync();
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
