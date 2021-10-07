using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Url);
            builder.Property(x => x.Title);

            builder.HasMany(x => x.Products);
            builder.Metadata.FindNavigation(nameof(Shop.Products))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
