using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.API.Policy
{
    public class PermissionsToUserClaimsFactory : UserClaimsPrincipalFactory<AuthUser>
    {
        private readonly IClaimsExtractor claimsExtractor;

        public PermissionsToUserClaimsFactory(
            UserManager<AuthUser> userManager,
            IClaimsExtractor claimsExtractor,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            this.claimsExtractor = claimsExtractor;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AuthUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var userId = identity.Claims.GetUserIdFromClaims();

            if (userId == Guid.Empty) return identity;

            var claims = await this.claimsExtractor.GetClaimForAuthUser(user);
            identity.AddClaims(claims);

            return identity;
        }
    }
}
