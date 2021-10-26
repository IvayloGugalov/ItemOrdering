using System;
using System.Collections.Generic;

namespace Ordering.Domain.Shared
{
    public abstract class Entity
    {
        // TODO: JsonIgnore not working here?
        public IReadOnlyList<IDomainEvent> DomainEvents => this.domainEvents;
        private readonly List<IDomainEvent> domainEvents = new();

        public Guid Id { get; init; }

        protected Entity() { }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            this.domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            this.domainEvents.Clear();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            if (this.Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return this.Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
