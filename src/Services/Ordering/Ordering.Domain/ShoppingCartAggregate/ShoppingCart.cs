using System;
using System.Collections.Generic;
using System.Linq;

using GuardClauses;

using Ordering.Domain.Shared;
using Ordering.Domain.ShopAggregate;

namespace Ordering.Domain.ShoppingCartAggregate
{
    public class ShoppingCart : Entity
    {
        public Guid CustomerId { get; }

        public IReadOnlySet<ProductAndAmount> ProductsAndAmount => this.productsAndAmount;
        private readonly HashSet<ProductAndAmount> productsAndAmount = new();

        private ShoppingCart() { }

        public ShoppingCart(Guid customerId)
        {
            this.Id = Guid.NewGuid();
            this.CustomerId = Guard.Against.NullOrEmpty(customerId, nameof(customerId));
        }

        /// <summary>
        /// Adds a product to the Shopping cart, or increases the amount of the product by 1, when it's already inside the Shopping cart.
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            Guard.Against.Null(product, nameof(product));

            var existingProduct = this.productsAndAmount.FirstOrDefault(x => x.ProductId == product.Id);

            if (existingProduct is not null)
            {
                existingProduct.IncreaseAmount(1);
            }
            else
            {
                this.productsAndAmount.Add(new ProductAndAmount(product.Id, product.OriginalPrice.Value, 1));
            }
        }

        public bool RemoveProduct(ProductAndAmount product)
        {
            return this.productsAndAmount.Remove(product);
        }

        public int AmountOfProduct(ProductAndAmount product)
        {
            return this.productsAndAmount.TryGetValue(product, out var retrievedProduct)
                ? retrievedProduct.Amount
                : 0;
        }

        public void Clear()
        {
            this.productsAndAmount.Clear();
        }
    }
}
