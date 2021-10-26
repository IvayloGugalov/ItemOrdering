using System;

using GuardClauses;

namespace Ordering.Domain.ShoppingCartAggregate
{
    public record Price
    {
        public double Value { get; }
        public Guid ItemId { get; }
        public DateTime DateCreated { get; }

        private Price() { }

        public Price(double value, Guid itemId)
        {
            this.Value = Guard.Against.NegativeOrZero(value, nameof(value));
            this.ItemId = Guard.Against.NullOrEmpty(itemId, nameof(itemId));
            this.DateCreated = DateTime.Now;
        }
    }
}
