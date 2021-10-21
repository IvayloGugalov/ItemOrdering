using System;

using GuardClauses;

namespace ItemOrdering.Domain.OrderAggregate
{
    public record OrderedProduct
    {
        public int Id { get; protected set; }

        public Guid ProductId { get; }
        public double Price { get; }
        public int Amount { get; }

        private OrderedProduct() { }

        public OrderedProduct(Guid productId, double price, int amount)
        {
            this.ProductId = Guard.Against.NullOrEmpty(productId, nameof(productId));
            this.Price = Guard.Against.NegativeOrZero(price, nameof(price));
            this.Amount = Guard.Against.NegativeOrZero(amount, nameof(amount));
        }

        public double CalculateTotalPrice => this.Price * this.Amount;
    }
}
