using System;

namespace ItemOrdering.Domain.Exceptions
{
    public class InvalidShoppingCartForCustomerException : Exception
    {
        public InvalidShoppingCartForCustomerException(string message)
            : base(message)
        {
        }
    }
}
