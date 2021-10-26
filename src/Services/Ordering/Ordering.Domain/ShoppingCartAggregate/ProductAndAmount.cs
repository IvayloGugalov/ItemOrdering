using System;

using GuardClauses;

namespace Ordering.Domain.ShoppingCartAggregate
{
    public class ProductAndAmount
    {
        public int Id { get; protected set; }

        public Guid ProductId { get; private set; }
        public double Price { get; private set; }
        public int Amount { get; private set; }

        private ProductAndAmount() { }

        public ProductAndAmount(Guid productId, double price, int amount)
        {
            this.ProductId = Guard.Against.NullOrEmpty(productId, nameof(productId));
            this.Price = Guard.Against.NegativeOrZero(price, nameof(price));
            this.Amount = Guard.Against.NegativeOrZero(amount, nameof(amount));
        }

        public void IncreaseAmount(int amountToIncrease)
        {
            this.Amount += amountToIncrease;
        }
    }
}