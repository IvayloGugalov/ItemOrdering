using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ItemOrdering.Domain.OrderAggregate;
using ItemOrdering.Domain.Shared;

namespace ItemOrdering.Domain.CustomerAggregate
{
    public class Customer : Entity
    {
        [Required]
        public string FirstName { get; private set; }
        [Required]
        public string LastName { get; private set; }
        [Required]
        public Address Address { get; private set; }
        [Required]
        public Email Email { get; private set; }

        public IReadOnlyList<Order> Orders => this.orders;
        private readonly List<Order> orders = new();

        private Customer() { }

        private Customer(string firstName, string lastName, Address address, Email email)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            // TODO: Add Address validation
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            // TODO: Add Email validation
            this.Email = !string.IsNullOrWhiteSpace(email.Value) ? email : throw new ArgumentNullException(nameof(email));
        }

        public Customer Create(string firstName, string lastName, Address address, Email email)
        {
            return new Customer(firstName: firstName, lastName: lastName, address: address, email: email);
        }

        public void AddOrder(Order order)
        {
            if (this.orders.Contains(order)) return;

            this.orders.Add(order);
        }

        public void RemoveOrder(Order order)
        {
            if (!this.orders.Contains(order)) return;

            this.orders.Remove(order);
        }

        public Customer UpdateCustomer()
        {



            return this;
        }
    }
}
