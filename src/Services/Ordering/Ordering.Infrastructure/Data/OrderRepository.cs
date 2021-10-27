using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Interfaces;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Shared;

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
        public async Task<IEnumerable<Order>> GetAllForCustomer(ISpecification<Order> specification)
        {
            return await this.context.Orders.Specify(specification).ToListAsync();
        }

        public async Task<Order> GetForCustomerId(ISpecification<Order> specification)
        {
            return await this.context.Orders.Specify(specification)
                .SingleOrDefaultAsync();
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
