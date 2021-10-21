using System;

namespace ItemOrdering.Domain.OrderAggregate
{
    /// <summary>
    /// Guid Id = Product.Id
    /// </summary>
    public record OrderedProduct(Guid ProductId, double Price, int Amount)
    {
        public int Id { get; protected set; }

        public double CalculateTotalPrice => this.Price * this.Amount;
    }
}
