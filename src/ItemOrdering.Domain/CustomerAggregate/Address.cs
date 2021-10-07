using System;

namespace ItemOrdering.Domain.CustomerAggregate
{
    public record Address
    {
        public string Country { get; }
        public string City { get; }
        public byte ZipCode { get; }
        public string Street { get; }
        public int StreetNumber { get; }

        public Address(string country, string city, byte zipCode, string street, int streetNumber)
        {
            this.Country = !string.IsNullOrEmpty(country) ? country : throw new ArgumentNullException(nameof(country));
            this.City = !string.IsNullOrWhiteSpace(city) ? city : throw new ArgumentNullException(nameof(city));
            this.ZipCode = zipCode > 0 ? zipCode : throw new ArgumentNullException(nameof(zipCode));
            this.Street = !string.IsNullOrWhiteSpace(street) ? street : throw new ArgumentNullException(nameof(street));
            this.StreetNumber = streetNumber >= 0 ? streetNumber : throw new ArgumentNullException(nameof(streetNumber));
        }

        public override string ToString()
        {
            return $"Country: {this.Country}, City: {this.City}, Zip Code: {this.ZipCode}, Street: {this.Street} {this.StreetNumber}";
        }
    }
}
