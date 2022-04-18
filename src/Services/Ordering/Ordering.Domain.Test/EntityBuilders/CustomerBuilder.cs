using GuidGenerator;

using Ordering.Domain.CustomerAggregate;
using Ordering.Domain.Shared;

namespace Ordering.Domain.Test.EntityBuilders
{
    public static class CustomerBuilder
    {
        public static Customer CreateCustomer(IGuidGeneratorService guidGenerator)
        {
            return new Customer(
                firstName: "Ivaylo",
                lastName: "Gugalov",
                address: new Address(
                    country: "Bulgaria",
                    city: "Sofia",
                    zipCode: 1000,
                    street: "4-ti Kilometyr",
                    streetNumber: 1),
                email: new Email("ivo_mail@mail.bg"),
                guidGenerator);
        }
    }
}
