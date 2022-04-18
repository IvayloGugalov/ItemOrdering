using System;
using System.Collections.Generic;
using System.Linq;

using GuardClauses;
using GuidGenerator;
using Ordering.Domain.Shared;

namespace Ordering.Domain.OrderAggregate
{
    public class Order : Entity
    {
        public Guid CustomerId { get; }
        public Address ShippingAddress { get; private set; }
        public DateTime Created { get; }

        public IReadOnlyCollection<OrderedProduct> OrderedProducts => this.orderedProducts.AsReadOnly();
        private readonly List<OrderedProduct> orderedProducts;

        private Order() { }

        public Order(
            Guid customerId,
            IEnumerable<OrderedProduct> orderedProducts,
            IGuidGeneratorService guidGenerator)
        {
            this.Id = guidGenerator.GenerateGuid();
            this.Created = DateTime.Now;
            this.CustomerId = Guard.Against.NullOrEmpty(customerId, nameof(customerId));

            this.orderedProducts = Guard.Against.NullOrEmpty(orderedProducts, nameof(orderedProducts)).ToList();
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
