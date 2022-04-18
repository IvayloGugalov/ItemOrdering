using System.Collections.Generic;

using GuidGenerator;

using Ordering.Domain.ShopAggregate;

namespace Ordering.Domain.Test.EntityBuilders
{
    public static class ProductBuilder
    {
        public static Product CreateProduct(IGuidGeneratorService guidGenerator)
        {
            return new Product(
                url: @"www.shop.url.com/product_1",
                title: "product_1",
                description: "This is the description",
                price: 49.99,
                new Shop(
                    url: "www.shop.url.com",
                    title: "my shop",
                    guidGenerator),
                guidGenerator);
        }

        public static Product CreateSpecificProduct(
            string url,
            string title,
            string description,
            double price,
            IGuidGeneratorService guidGenerator)
        {
            return new Product(
                url: url,
                title: title,
                description: description,
                price: price,
                new Shop(
                    url: "www.shop.url.com",
                    title: "my shop",
                    guidGenerator),
                guidGenerator);
        }

        public static IEnumerable<Product> CreateProducts(int range, IGuidGeneratorService guidGenerator)
        {
            for (var i = 1; i <= range; i++)
            {
                yield return CreateSpecificProduct(
                    url: $"www.shop.url.com//product_{i}",
                    title: $"product_{i}",
                    description: $"This is the description {i}",
                    price: 1.05 * i,
                    guidGenerator);
            }
        }
    }
}
