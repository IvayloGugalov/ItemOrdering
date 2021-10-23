using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.ShopAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.ToTable("Shops");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Url)
                .IsRequired();
            builder.Property(x => x.Title)
                .IsRequired();

            builder.HasMany(x => x.Products);
            builder.Metadata.FindNavigation(nameof(Shop.Products))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
