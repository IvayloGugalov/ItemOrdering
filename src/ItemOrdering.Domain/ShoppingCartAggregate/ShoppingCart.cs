using System;
using System.Collections.Generic;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public class ShoppingCart : Entity
    {
        public Guid CustomerId { get; }

        public IReadOnlyDictionary<Product, int> ProductsAndAmount => this.productsAndAmount;
        private readonly Dictionary<Product, int> productsAndAmount = new();

        private ShoppingCart() { }

        private ShoppingCart(Guid customerId)
        {
            this.CustomerId = customerId != Guid.Empty ? customerId : throw new ArgumentNullException(nameof(customerId));
        }

        public static ShoppingCart Create(Guid customerId)
        {
            return new ShoppingCart(customerId);
        }

        public void AddProduct(Product product)
        {
            if (this.productsAndAmount.ContainsKey(product))
            {
                this.productsAndAmount[product] += 1;
            }
            else
            {
                this.productsAndAmount.Add(product, 1);
            }
        }

        public bool RemoveProduct(Product product)
        {
            return this.productsAndAmount.Remove(product);
        }

        public int AmountOfProduct(Product product)
        {
            return this.productsAndAmount.TryGetValue(product, out var amount)
                ? amount
                : 0;
        }

        public void Clear()
        {
            this.productsAndAmount.Clear();
        }
    }
}
