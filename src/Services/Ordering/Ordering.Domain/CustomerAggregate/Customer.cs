using System;
using System.Collections.Generic;

using GuardClauses;

using Ordering.Domain.Exceptions;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.Shared;

namespace Ordering.Domain.CustomerAggregate
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
            this.FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            this.LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            this.Address = Guard.Against.Null(address, nameof(address));
            this.Email = Guard.Against.Null(email, nameof(email));
        }

        public void SetShoppingCart(Guid shoppingCartId)
        {
            this.ShoppingCartId = this.ShoppingCartId == Guid.Empty
                ? shoppingCartId
                : throw new ShoppingCartMappedException("Customer already has a shopping cart instance.");
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
}
