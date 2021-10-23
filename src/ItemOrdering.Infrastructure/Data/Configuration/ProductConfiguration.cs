using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.ShopAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired();
            builder.Property(x => x.Description)
                .IsRequired();
            builder.Property(x => x.Url)
                .IsRequired();

            builder.HasOne(x => x.Shop)
                .WithMany(x => x.Products)
                .IsRequired();

            builder.OwnsOne(x => x.OriginalPrice)
                .Property(x => x.Value)
                .HasColumnType("float")
                .IsRequired();
        }
    }
}
