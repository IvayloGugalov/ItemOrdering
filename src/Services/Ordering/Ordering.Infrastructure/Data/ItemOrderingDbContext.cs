using Microsoft.EntityFrameworkCore;

using Ordering.Domain.CustomerAggregate;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Infrastructure.Data
{
    public class ItemOrderingDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts{ get; set; }

        public ItemOrderingDbContext(DbContextOptions<ItemOrderingDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemOrderingDbContext).Assembly);
        }
    }
}
