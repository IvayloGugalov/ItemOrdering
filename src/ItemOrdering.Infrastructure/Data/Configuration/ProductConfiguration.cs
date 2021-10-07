using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Title);
            builder.Property(x => x.Description);
            builder.Property(x => x.Url);

            builder.HasOne(x => x.Shop)
                .WithMany(x => x.Products);

            builder.OwnsOne(x => x.OriginalPrice)
                .Property(x => x.Value)
                .HasColumnType("float");
        }
    }
}
