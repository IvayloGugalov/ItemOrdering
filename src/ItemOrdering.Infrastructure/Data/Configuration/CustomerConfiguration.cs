using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ItemOrdering.Domain.CustomerAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.FirstName)
                .IsRequired();
            builder.Property(x => x.LastName)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasConversion(email => email.Value, value => new Email(value))
                .HasMaxLength(220)
                .IsRequired();

            builder.OwnsOne(
                x => x.Address,
                sa =>
                {
                    sa.Property(p => p.Country).HasColumnName("CustomerCountry");
                    sa.Property(p => p.City).HasColumnName("CustomerCity");
                    sa.Property(p => p.ZipCode).HasColumnName("CustomerZipCode");
                    sa.Property(p => p.Street).HasColumnName("CustomerStreet");
                    sa.Property(p => p.StreetNumber).HasColumnName("CustomerStreetNumber");
                });
            builder.Navigation(x => x.Address)
                .IsRequired();

        }
    }
}
