using GuardClauses;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.API.Models
{
    public record Address
    {
        [BsonRequired]
        public string Country { get; }

        [BsonRequired]
        public string City { get; }

        [BsonRequired]
        public int ZipCode { get; }

        [BsonRequired]
        public string Street { get; }

        [BsonRequired]
        public int StreetNumber { get; }

        public Address(string country, string city, int zipCode, string street, int streetNumber)
        {
            this.Country = Guard.Against.NullOrWhiteSpace(country, nameof(country));
            this.City = Guard.Against.NullOrWhiteSpace(city, nameof(city));
            this.ZipCode = Guard.Against.NegativeOrZero(zipCode, nameof(zipCode));
            this.Street = Guard.Against.NullOrWhiteSpace(street, nameof(street));
            this.StreetNumber = Guard.Against.NegativeOrZero(streetNumber, nameof(streetNumber));
        }

        public override string ToString()
        {
            return $"Country: {this.Country}, City: {this.City}, Zip Code: {this.ZipCode}, Street: {this.Street} {this.StreetNumber}";
        }
    }
}
