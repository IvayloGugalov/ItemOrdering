using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ItemOrderingDbContext context;

        public ProductRepository(ItemOrderingDbContext context)
        {
            this.context = context;
        }

        public async Task<Product> GetByIdAsync(Guid productId)
        {
            return await this.context.Products.SingleOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<Product> AddAsync(Product product)
        {
            await this.context.AddAsync(product);
            await this.context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            this.context.Products.Remove(product);
            await this.context.SaveChangesAsync();
        }
    }
}
