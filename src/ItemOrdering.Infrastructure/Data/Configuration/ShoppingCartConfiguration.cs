using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data.Configuration
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            var valueComparer = new ValueComparer<IReadOnlyDictionary<Product, int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToDictionary(x => x.Key, x => x.Value));

            //One important thing I have noticed, however, is that when updating the entity and changing items in the dictionary, the EF change tracking does not pick up on the fact that the dictionary was updated, so you will need to explicitly call the Update method on the DbSet<> to set the entity to modified in the change tracker.
            builder.Property(x => x.ProductsAndAmount)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<IReadOnlyDictionary<Product, int>>(v))
                .HasField("productsAndAmount")
                .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
                .Metadata.SetValueComparer(valueComparer);
        }
    }
}
