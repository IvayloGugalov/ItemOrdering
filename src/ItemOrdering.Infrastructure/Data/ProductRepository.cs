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

        public async Task<Product> GetProductAsync(Guid productId)
        {
            return await this.context.Products.SingleOrDefaultAsync(x => x.Id == productId);
        }
    }
}
