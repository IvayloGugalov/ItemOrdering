using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Infrastructure.Data.Configuration
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.HasMany(x => x.ProductsAndAmount);
            builder.Metadata.FindNavigation(nameof(ShoppingCart.ProductsAndAmount))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
