using System;

namespace ItemOrdering.Domain.OrderAggregate
{
    /// <summary>
    /// Guid Id = Product.Id
    /// </summary>
    public record OrderedProduct(Guid Id, double Price, int Amount)
    {
        public double CalculateTotalPrice => this.Price * this.Amount;
    }
}
