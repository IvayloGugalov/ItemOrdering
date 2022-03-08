using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IClaimsExtractor
    {
        /// <summary>
        /// Retrieve the claims for the user from DB.
        /// Function is called twice:
        ///   1. On Access Token creation
        ///   2. On Sign In
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<Claim>> GetClaimForAuthUser(AuthUser user);
    }
}
