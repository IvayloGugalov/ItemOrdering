using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.OrderAggregate
{
    public class Order : Entity
    {
        [Required]
        public Guid CustomerId { get; }
        [Required]
        public Address ShippingAddress { get; private set; }
        [Required]
        public DateTime Created { get; }

        public IReadOnlyList<Product> Products => this.products.AsReadOnly();
        private readonly List<Product> products = new();

        private Order() { }

        private Order(Guid customerId)
        {
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
            this.CustomerId = customerId != Guid.Empty ? customerId : throw new ArgumentNullException(nameof(customerId));
        }

        public static Order CreateOrder(Guid customerId)
        {
            return new Order(customerId);
        }

        public void AddProduct(Product product)
        {
            if (this.products.Contains(product)) return;

            this.products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            if (!this.products.Contains(product)) return;

            this.products.Remove(product);
        }

        public void SetShippingAddress(Address address)
        {
            this.ShippingAddress = address;
        }

        public double CalculateTotalPrice()
        {
             return this.products.Sum(item => item.OriginalPrice.Value);
        }
    }
}
