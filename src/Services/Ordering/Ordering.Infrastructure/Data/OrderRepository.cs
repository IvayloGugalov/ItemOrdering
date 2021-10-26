using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Ordering.Domain.OrderAggregate;
using Ordering.Domain.OrderAggregate.Specifications;

namespace Ordering.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ItemOrderingDbContext context;

        public OrderRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        // We are loading the full navigation properties here and all of the contents.
        // .AsNoTracking(), .IgnoreAutoIncludes() are not working to skip the navigations.
        public async Task<List<Order>> GetAllByCustomerIdAsync(Guid customerId)
        {
            return await this.context.Orders
                .Where(x => x.CustomerId == customerId)
                .OrderBy(x => x.Created)
                .ToListAsync();
        }

        public async Task<Order> GetByCustomerIdWithProductsAsync(Guid customerId)
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
