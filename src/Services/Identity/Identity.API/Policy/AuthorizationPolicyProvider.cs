using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Identity.API.Policy
{
    /// <summary>
    /// Resolving policies dynamically. Registered and fired upon StartUp
    /// Policies are fragments you can add to an authorization attribute that confirm whether a given HttpContext meets the requirements of the attribute, beyond simply being authenticated or not.
    /// </summary>
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return await base.GetPolicyAsync(policyName)
                   ?? new AuthorizationPolicyBuilder()
                       .AddRequirements(new PermissionRequirement(policyName))
                       .Build();
        }
    }
}
