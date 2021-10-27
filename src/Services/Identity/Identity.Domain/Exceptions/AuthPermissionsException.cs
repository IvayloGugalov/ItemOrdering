using System;

namespace Identity.Domain.Exceptions
{
    public class AuthPermissionsException : Exception
    {
        public AuthPermissionsException(string message)
            : base(message)
        {
        }
    }
}
