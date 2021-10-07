using ItemOrdering.Domain.CustomerAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.OrderAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(item => item.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Created)
                .HasColumnType("date")
                .ValueGeneratedNever()
                .IsRequired();

            builder.OwnsOne(
                    x => x.ShippingAddress,
                    sa =>
                    {
                        sa.Property(p => p.Country).HasColumnName("ShippingCountry");
                        sa.Property(p => p.City).HasColumnName("ShippingCity");
                        sa.Property(p => p.ZipCode).HasColumnName("ShippingZipCode");
                        sa.Property(p => p.Street).HasColumnName("ShippingStreet");
                        sa.Property(p => p.StreetNumber).HasColumnName("ShippingStreetNumber");
                    });
            builder.Navigation(x => x.ShippingAddress)
                .IsRequired();

            builder.HasMany(x => x.Products);
            builder.Metadata.FindNavigation(nameof(Order.Products))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Customer>()
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();
        }
    }
}
