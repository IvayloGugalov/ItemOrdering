using System;

namespace Ordering.Domain.Exceptions
{
    public class InvalidShoppingCartForCustomerException : Exception
    {
        public InvalidShoppingCartForCustomerException(string message)
            : base(message)
        {
        }
    }
}
