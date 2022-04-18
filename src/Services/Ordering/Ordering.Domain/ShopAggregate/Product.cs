using System;

using GuardClauses;
using GuidGenerator;
using Ordering.Domain.Shared;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Domain.ShopAggregate
{
    public class Product : Entity
    {
        public string Url { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Price OriginalPrice { get; private set; }
        public Shop Shop { get; }

        private Product() { }

        public Product(
            string url,
            string title,
            string description,
            double price,
            Shop shop,
            IGuidGeneratorService guidGenerator)
        {
            this.Id = guidGenerator.GenerateGuid();
            this.Url = Guard.Against.NullOrWhiteSpace(url, nameof(url));
            this.Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
            this.Description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
            this.OriginalPrice = new Price(price, this.Id);
            this.Shop = Guard.Against.Null(shop, nameof(shop));
        }

        protected Product UpdateProduct(string title, string description, double price)
        {
            this.Title = title;
            this.Description = description;
            this.OriginalPrice = new Price(price, this.Id);

            return this;
        }
    }
}
