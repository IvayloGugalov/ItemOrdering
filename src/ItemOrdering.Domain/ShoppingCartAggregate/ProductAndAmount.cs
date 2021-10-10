using System;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public class ProductAndAmount
    {
        public int Id{ get; protected set; }
        public Guid ProductId { get; private set; }
        public double Price { get; private set; }
        public int Amount{ get; private set; }

        private ProductAndAmount() { }

        public ProductAndAmount(Guid productId, double price, int amount)
        {
            this.ProductId = productId != Guid.Empty ? productId : throw new ArgumentNullException(nameof(productId));
            this.Price = price > 0 ? price : throw new ArgumentNullException(nameof(price));
            this.Amount = amount > 0 ? amount : throw new ArgumentNullException(nameof(amount));
        }

        public void IncreaseAmount(int amountToIncrease)
        {
            this.Amount += amountToIncrease;
        }
    }
}