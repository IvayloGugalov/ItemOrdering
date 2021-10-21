using System;
using System.Collections.Generic;

using GuardClauses;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public class Shop : Entity
    {
        public string Url { get; }
        public string Title { get; }

        public IReadOnlyList<Product> Products => this.products?.AsReadOnly();
        private readonly List<Product> products = new();

        private Shop() { }

        public Shop(string url, string title)
        {
            this.Id = Guid.NewGuid();
            this.Url = Guard.Against.NullOrWhiteSpace(url, nameof(url));
            this.Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
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
    }
}
