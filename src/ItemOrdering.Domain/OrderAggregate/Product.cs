using System;
using System.ComponentModel.DataAnnotations;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.OrderAggregate
{
    public class Product : Entity
    {
        [Required]
        public string Title { get; private set; }
        [Required]
        public string Description { get; private set; }
        [Required]
        public Price OriginalPrice { get; private set; }
        [Required]
        public string Url { get; }
        [Required]
        public Shop Shop { get; }

        private Product() { }

        private Product(string url, string title, string description, double price, Shop shop)
        {
            this.Id = Guid.NewGuid();
            this.Url = !string.IsNullOrWhiteSpace(url) ? url : throw new ArgumentNullException(nameof(url));
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
            this.Description = description;
            this.OriginalPrice = price >= 0 ? new Price(price, this.Id) : throw new ArgumentNullException(nameof(price));
            this.Shop = shop ?? throw new ArgumentNullException(nameof(shop));
        }

        public static Product CreateProduct(string url, string title, string description, double price, Shop shop)
        {
            return new Product(url, title, description, price, shop);
        }

        protected Product UpdateProduct(Product item)
        {
            // TODO: Update item
            return this;
        }
    }
}
