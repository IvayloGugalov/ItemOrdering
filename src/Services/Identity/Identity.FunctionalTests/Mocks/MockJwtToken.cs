using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using Microsoft.IdentityModel.Tokens;

namespace Identity.Functional.Tests.Mocks
{
    public static class MockJwtToken
    {
        public static string Issuer => "https://localhost:5005";
        public static string Audience => Issuer;
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler tokenHandler = new();
        private static readonly RandomNumberGenerator random = RandomNumberGenerator.Create();
        private static readonly byte[] secretKey = new byte[32];

        static MockJwtToken()
        {
            random.GetBytes(secretKey);
            SecurityKey = new SymmetricSecurityKey(secretKey) { KeyId = Guid.NewGuid().ToString() };
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public static string GenerateJwtToken(IEnumerable<Claim> claims) => 
            tokenHandler.WriteToken(
                new JwtSecurityToken(Issuer,
                    null,
                    claims,
                    null,
                    DateTime.UtcNow.AddMinutes(20),
                    SigningCredentials));
    }
}
