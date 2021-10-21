using System;
using System.Collections.Generic;
using System.Linq;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.OrderAggregate
{
    public class Order : Entity
    {
        public Guid CustomerId { get; }
        public Address ShippingAddress { get; private set; }
        public DateTime Created { get; }

        public IReadOnlyCollection<OrderedProduct> OrderedProducts => this.orderedProducts.AsReadOnly();
        private readonly List<OrderedProduct> orderedProducts;

        private Order() { }

        public Order(Guid customerId, List<OrderedProduct> orderedProducts)
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
            this.CustomerId = customerId != Guid.Empty ? customerId : throw new ArgumentNullException(nameof(customerId));

            this.orderedProducts = orderedProducts.Any() ? orderedProducts : throw new ArgumentNullException(nameof(orderedProducts));
        }

        public void SetShippingAddress(Address address)
        {
            this.ShippingAddress = address;
        }

        public double CalculateTotalPrice()
        {
             return this.OrderedProducts.Sum(item => item.CalculateTotalPrice);
        }
    }
}
