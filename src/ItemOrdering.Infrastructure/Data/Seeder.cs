using System.Linq;

using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.Shared;
using ItemOrdering.Domain.ShopAggregate;
using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public static class Seeder
    {
        public static void Initialize(ItemOrderingDbContext dbContext)
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
                        email: new Email("elon_musk_fake@mail.bg")));

                dbContext.ShoppingCarts.Add(new ShoppingCart(customer.Entity.Id));
            }

            if (!dbContext.Products.Any())
            {
                var shop = new Shop(@"https:\\shop_2.com", title: "shop_2");
                var products = new[]
                {
                    new Product(
                        url: @"https:\\product_12", title: "product_12", description: "This is the description", price: 549.99, shop),
                    new Product(
                        url: @"https:\\product_22", title: "product_22", description: "This is the description", price: 589.99, shop),
                    new Product(
                        url: @"https:\\product_32", title: "product_32", description: "This is the description", price: 5100, shop),
                    new Product(
                        url: @"https:\\product_42", title: "product_42", description: "This is the description", price: 50.99, shop),
                };

                dbContext.Products.AddRange(products);
            }

            dbContext.SaveChanges();
        }

        public static Customer CustomerWithCartAndProducts(ItemOrderingDbContext dbContext)
        {
            var customer = CreateCustomer();
            var shoppingCart = new ShoppingCart(customer.Id);
            var products = CreateProducts();

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

        public static Customer CustomerWithMultipleOrders(ItemOrderingDbContext dbContext, int orderCount)
        {
            var customer = CreateCustomer();
            var shoppingCart = new ShoppingCart(customer.Id);
            var products = CreateProducts();

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
                dbContext.Orders.Add(new Order(shoppingCart.CustomerId, orderedProducts));
            }

            dbContext.SaveChanges();
            return customer;
        }

        private static Customer CreateCustomer() =>
            new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"));

        private static Product[] CreateProducts()
        {
            var shop = new Shop(@"https:\\shop_1.com", title: "shop_1");
            var products = new[]
            {
                new Product(
                    url: @"https:\\product_1", title: "product_1", description: "This is the description", price: 49.99, shop),
                new Product(
                    url: @"https:\\product_2", title: "product_2", description: "This is the description", price: 89.99, shop),
                new Product(
                    url: @"https:\\product_3", title: "product_3", description: "This is the description", price: 100, shop),
                new Product(
                    url: @"https:\\product_4", title: "product_4", description: "This is the description", price: 0.99, shop),
            };

            return products;
        }
    }
}
