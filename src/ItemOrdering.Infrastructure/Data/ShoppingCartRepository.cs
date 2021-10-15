using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ItemOrdering.Domain.ShoppingCartAggregate;
using ItemOrdering.Domain.ShoppingCartAggregate.Specifications;

namespace ItemOrdering.Infrastructure.Data
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

        public async Task UpdateAsync(ShoppingCart shoppingCart)
        {
            this.context.ShoppingCarts.Attach(shoppingCart);
            this.context.Entry(shoppingCart).State = EntityState.Modified;

            await this.context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> FindByCustomerIncludeProducts(Guid customerId)
        {
            return await this.context.ShoppingCarts
                .GetProductsForCart(customerId)
                .SingleOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task DeleteAsync(ShoppingCart shoppingCart)
        {
            this.context.ShoppingCarts.Remove(shoppingCart);
            await this.context.SaveChangesAsync();
        }
    }
}
