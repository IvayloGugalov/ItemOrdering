using System.Linq;

using ItemOrdering.Domain.CustomerAggregate;
using ItemOrdering.Domain.Shared;
using ItemOrdering.Domain.ShoppingCartAggregate;

namespace ItemOrdering.Infrastructure.Data
{
    public static class Seeder
    {
        public static void Initialize(ItemOrderingDbContext context)
        {
            if (!context.Customers.Any())
            {
                context.Customers.Add(
                    Customer.Create(
                        firstName: "Ivaylo",
                        lastName: "Gugalov",
                        address: new Address(
                            country: "Bulgaria",
                            city: "Sofia",
                            zipCode: 1000,
                            street: "4-ti Kilometyr",
                            streetNumber: 1),
                        email: new Email("ivo_mail@mail.bg")));
            }

            if (!context.Products.Any())
            {
                var shop = Shop.Create(@"https:\\shop_1.com", title: "shop_1");
                var products = new[]
                {
                    Product.CreateProduct(
                        url: @"https:\\product_1", title: "product_1", description: "This is the description", price: 49.99, shop),
                    Product.CreateProduct(
                        url: @"https:\\product_2", title: "product_2", description: "This is the description", price: 89.99, shop),
                    Product.CreateProduct(
                        url: @"https:\\product_3", title: "product_3", description: "This is the description", price: 100, shop),
                    Product.CreateProduct(
                        url: @"https:\\product_4", title: "product_4", description: "This is the description", price: 0.99, shop),
                };

                context.Products.AddRange(products);
            }

            context.SaveChanges();
        }
    }
}
