using System;

namespace ItemOrdering.Domain.OrderAggregate
{
    public record OrderedProduct(Guid Id, double Price, int Amount)
    {
        public double CalculateTotalPrice => this.Price * this.Amount;
    }
}
