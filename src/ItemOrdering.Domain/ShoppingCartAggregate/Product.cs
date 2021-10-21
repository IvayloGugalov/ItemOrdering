using System;

using GuardClauses;

using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.ShoppingCartAggregate
{
    public class Product : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Url { get; }
        public Price OriginalPrice { get; private set; }
        public Shop Shop { get; }

        private Product() { }

        public Product(string url, string title, string description, double price, Shop shop)
        {
            this.Id = Guid.NewGuid();
            this.Url = Guard.Against.NullOrWhiteSpace(url, nameof(url));
            this.Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
            this.Description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
            this.OriginalPrice = new Price(price, this.Id);
            this.Shop = Guard.Against.Null(shop, nameof(shop));
        }

        protected Product UpdateProduct(Product item)
        {
            // TODO: Update item
            return this;
        }
    }
}
