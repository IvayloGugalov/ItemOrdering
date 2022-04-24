using System.Linq;

using GuidGenerator;

using Ordering.Domain.CustomerAggregate;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Shared;
using Ordering.Domain.ShopAggregate;
using Ordering.Domain.ShoppingCartAggregate;

namespace Ordering.Infrastructure.Data
{
    public static class Seeder
    {
        public static void Initialize(ItemOrderingDbContext dbContext, IGuidGeneratorService guidGenerator)
        {
            if (!dbContext.Customers.Any())
            {
                var customer = dbContext.Customers.Add(
                    new Customer(
                        firstName: "Elon",
                        lastName: "Musk",
                        address: new Address(
                            country: "USA",
                            city: "Los Angelis",
                            zipCode: 50100,
                            street: "Tom Soyer Str.",
                            streetNumber: 6),
                        email: new Email("elon_musk_fake@mail.bg"),
                        guidGenerator));

                dbContext.ShoppingCarts.Add(new ShoppingCart(customer.Entity.Id, guidGenerator));
            }

            if (!dbContext.Products.Any())
            {
                var shop = new Shop(@"https:\\shop_2.com", title: "shop_2", guidGenerator);
                var products = new[]
                {
                    new Product(
                        url: @"https:\\product_12", title: "product_12", description: "This is the description", price: 549.99, shop, guidGenerator),
                    new Product(
                        url: @"https:\\product_22", title: "product_22", description: "This is the description", price: 589.99, shop, guidGenerator),
                    new Product(
                        url: @"https:\\product_32", title: "product_32", description: "This is the description", price: 5100, shop, guidGenerator),
                    new Product(
                        url: @"https:\\product_42", title: "product_42", description: "This is the description", price: 50.99, shop, guidGenerator),
                };

                dbContext.Products.AddRange(products);
            }

            dbContext.SaveChanges();
        }

        public static Customer CustomerWithCartAndProducts(ItemOrderingDbContext dbContext, IGuidGeneratorService guidGenerator)
        {
            var customer = CreateCustomer(guidGenerator);
            var shoppingCart = new ShoppingCart(customer.Id, guidGenerator);
            var products = CreateProducts(guidGenerator);

            dbContext.Customers.Add(customer);
            dbContext.Products.AddRange(products);

            foreach (var product in products)
            {
                shoppingCart.AddProduct(product);
            }
            dbContext.ShoppingCarts.Add(shoppingCart);
            dbContext.SaveChanges();

            return customer;
        }

        public static Customer CustomerWithMultipleOrders(ItemOrderingDbContext dbContext, IGuidGeneratorService guidGenerator, int orderCount)
        {
            var customer = CreateCustomer(guidGenerator);
            var shoppingCart = new ShoppingCart(customer.Id, guidGenerator);
            var products = CreateProducts(guidGenerator);

            dbContext.Customers.Add(customer);
            dbContext.Products.AddRange(products);

            foreach (var product in products)
            {
                shoppingCart.AddProduct(product);
            }
            dbContext.ShoppingCarts.Add(shoppingCart);

            for (var i = 0; i < orderCount; i++)
            {
                var orderedProducts = shoppingCart.ProductsAndAmount
                    .Select(productAndAmount => new OrderedProduct(productAndAmount.ProductId, productAndAmount.Price, productAndAmount.Amount))
                    .ToList();
                dbContext.Orders.Add(new Order(shoppingCart.CustomerId, orderedProducts, guidGenerator));
            }

            dbContext.SaveChanges();
            return customer;
        }

        private static Customer CreateCustomer(IGuidGeneratorService guidGenerator) =>
            new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"),
                guidGenerator);

        private static Product[] CreateProducts(IGuidGeneratorService guidGenerator)
        {
            var shop = new Shop(@"https:\\shop_1.com", title: "shop_1", guidGenerator);
            var products = new[]
            {
                new Product(
                    url: @"https:\\product_1", title: "product_1", description: "This is the description", price: 49.99, shop, guidGenerator),
                new Product(
                    url: @"https:\\product_2", title: "product_2", description: "This is the description", price: 89.99, shop, guidGenerator),
                new Product(
                    url: @"https:\\product_3", title: "product_3", description: "This is the description", price: 100, shop, guidGenerator),
                new Product(
                    url: @"https:\\product_4", title: "product_4", description: "This is the description", price: 0.99, shop, guidGenerator),
            };

            return products;
        }
    }
}
