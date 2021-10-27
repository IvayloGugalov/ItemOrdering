using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Ordering.Domain.Interfaces;
using Ordering.Domain.Shared;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Infrastructure.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ItemOrderingDbContext context;

        public ShoppingCartRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ShoppingCart shoppingCart)
        {
            await this.context.ShoppingCarts.AddAsync(shoppingCart);
            await this.context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> FindAsync(Guid id)
        {
            return await this.context.ShoppingCarts.FindAsync(id);
        }

        public async Task UpdateAsync(ShoppingCart shoppingCart)
        {
            this.context.ShoppingCarts.Attach(shoppingCart);
            this.context.Entry(shoppingCart).State = EntityState.Modified;

            await this.context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> FindByCustomerAsync(ISpecification<ShoppingCart> specification)
        {
            return await this.context.ShoppingCarts.Specify(specification)
                .SingleOrDefaultAsync();
        }

        public async Task DeleteAsync(ShoppingCart shoppingCart)
        {
            this.context.ShoppingCarts.Remove(shoppingCart);
            await this.context.SaveChangesAsync();
        }
    }
}
