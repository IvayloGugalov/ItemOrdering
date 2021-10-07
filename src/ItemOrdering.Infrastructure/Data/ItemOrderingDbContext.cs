﻿using Microsoft.EntityFrameworkCore;

using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public class ItemOrderingDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

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
