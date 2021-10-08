using System;
using System.Collections.Generic;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.OrderAggregate
{
    public class Shop : Entity
    {
        public string Url { get; }
        public string Title { get; }

        public IReadOnlyList<Product> Products => this.products?.AsReadOnly();
        private readonly List<Product> products = new();

        private Shop() { }

        private Shop(string url, string title)
        {
            this.Id = Guid.NewGuid();
            this.Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        }

        public static Shop Create(string url, string title)
        {
            return new Shop(url, title);
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
