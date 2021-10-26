using System;

namespace Ordering.Domain.Exceptions
{
    public class ShoppingCartMappedException : Exception
    {
        public ShoppingCartMappedException(string message)
            : base(message)
        {
        }
    }
}