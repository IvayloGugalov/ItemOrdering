using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ItemOrderingDbContext context;

        public ShoppingCartRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        public async Task AddShoppingCart(ShoppingCart shoppingCart)
        {
            await this.context.ShoppingCarts.AddAsync(shoppingCart);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            this.context.ShoppingCarts.Attach(shoppingCart);

            await Task.FromResult(this.context.ShoppingCarts.Update(shoppingCart));
            await this.context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartForCustomer(Guid customerId)
        {
            return await this.context.ShoppingCarts.SingleOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<ShoppingCart> GetShoppingCartWithProducts(Guid customerId)
        {
            return await this.context.ShoppingCarts.Include(x => x.ProductsAndAmount).SingleOrDefaultAsync(x => x.CustomerId == customerId);
        }
    }
}
