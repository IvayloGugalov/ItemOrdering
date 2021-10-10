using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ProductAndAmountConfiguration : IEntityTypeConfiguration<ProductAndAmount>
    {
        public void Configure(EntityTypeBuilder<ProductAndAmount> builder)
        {
            builder.ToTable("ProductsAndAmount");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ProductId)
                .IsRequired();
            builder.Property(x => x.Price)
                .IsRequired();
            builder.Property(x => x.Amount)
                .IsRequired();

            builder.HasOne<ShoppingCart>()
                .WithMany(x => x.ProductsAndAmount)
                .IsRequired();
        }
    }
}
