using System;

namespace Identity.Functional.Tests
{
    public class TestAuthUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Permissions { get; set; }
    }
}