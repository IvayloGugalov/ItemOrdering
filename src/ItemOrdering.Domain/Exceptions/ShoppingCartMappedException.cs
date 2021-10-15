using System;

namespace ItemOrdering.Domain.Exceptions
{
    public class ShoppingCartMappedException : Exception
    {
        public ShoppingCartMappedException(string message)
            : base(message)
        {
        }
    }
}