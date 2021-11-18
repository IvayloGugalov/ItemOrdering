using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Identity.API.Extensions
{
    public static class ClaimsExtensions
    {
        /// <summary>
        /// This returns the UserId from the current user (
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>The UserId, or null if not logged in</returns>
        public static Guid GetUserIdFromClaims(this IEnumerable<Claim> claims)
        {
            var unparsedId = claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(unparsedId, out var userId)
                ? userId
                : Guid.Empty;
        }
    }
}
