using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Identity.Permissions;
using Identity.Tokens;
using Identity.Tokens.Interfaces;

namespace Identity.API.Policy
{
    public class PermissionPolicyHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IAccessTokenValidator accessTokenValidator;
        private readonly HttpContext httpContext;

        public PermissionPolicyHandler(IHttpContextAccessor httpContextAccessor, IAccessTokenValidator accessTokenValidator)
        {
            this.accessTokenValidator = accessTokenValidator;
            this.httpContext = httpContextAccessor.HttpContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (!this.httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader)) return Task.CompletedTask;

            // Validate the actual token without the Bearer in front ('Bearer df6sd768gc....')
            var accessToken = authHeader.ToString().Split(' ')[1];

            var result = this.accessTokenValidator.Validate(accessToken, out var principal);

            switch (result)
            {
                case TokenValidationResult.Success:
                    break;
                case TokenValidationResult.TokenExpired:
                case TokenValidationResult.EncryptionKeyNotFound:
                case TokenValidationResult.Unknown:
                    return Task.CompletedTask;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var permissionsClaim = principal.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);

            if (permissionsClaim == null) return Task.CompletedTask;

            if (permissionsClaim.IsPermissionAllowed(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
