using System;

namespace ItemOrdering.Domain.OrderAggregate
{
    public record Price
    {
        public double Value { get; }
        public Guid ItemId { get; }
        public DateTime DateCreated { get; }

        public Price(double value, Guid itemId)
        {
            this.Value = value > 0 ? value : throw new NullReferenceException("Price value");
            this.ItemId = itemId != Guid.Empty ? itemId : throw new NullReferenceException(nameof(itemId));
            this.DateCreated = DateTime.Now;
        }
    }
}
