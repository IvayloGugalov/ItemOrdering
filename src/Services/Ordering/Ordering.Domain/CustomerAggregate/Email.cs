using System;
using System.Text.RegularExpressions;

using GuardClauses;

namespace Ordering.Domain.CustomerAggregate
{
    public record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(Email));

            if (value.Length > 220)
            {
                throw new ArgumentOutOfRangeException(nameof(value), message: "Email value is longer than 220 characters");
            }

            if (!Regex.IsMatch(value, @"^(.+)@(.+)$"))
            {
                throw new ArgumentException($"Email value: {value}, does not match an acceptable email.");
            }

            this.Value = value;
        }

        public override string ToString() => this.Value;
    }
}
