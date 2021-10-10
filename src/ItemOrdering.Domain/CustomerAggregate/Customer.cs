using System;
using System.Collections.Generic;

using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.CustomerAggregate
{
    public class Customer : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Address Address { get; private set; }
        public Email Email { get; private set; }

        public Guid ShoppingCartId { get; private set; }

        public IReadOnlyCollection<Order> Orders => this.orders;
        private readonly List<Order> orders = new();

        private Customer() { }

        public Customer(string firstName, string lastName, Address address, Email email)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            // TODO: Add Address validation
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            // TODO: Add Email validation
            this.Email = !string.IsNullOrWhiteSpace(email.Value) ? email : throw new ArgumentNullException(nameof(email));
        }

        public void CreateShoppingCart(Guid shoppingCartId)
        {
            this.ShoppingCartId = this.ShoppingCartId == Guid.Empty
                ? shoppingCartId
                : throw new ShoppingCartMappedException();
        }

        public void AddOrder(Order order)
        {
            if (this.orders.Contains(order)) return;

            this.orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return this.orders.Remove(order);
        }

        public Customer UpdateCustomer()
        {
            return this;
        }
    }

    public class ShoppingCartMappedException : Exception
    {
        public ShoppingCartMappedException()
            : base(message: "Customer already has a shopping cart instance.")
        {
        }
    }
}
