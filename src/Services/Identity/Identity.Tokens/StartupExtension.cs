using Microsoft.Extensions.DependencyInjection;

using Identity.Tokens.Interfaces;
using Identity.Tokens.TokenGenerators;
using Identity.Tokens.TokenValidators;

namespace Identity.Tokens
{
    public static class StartupExtension
    {
        public static IServiceCollection AddTokenServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddTransient<IAccessTokenGenerator, AccessTokenGenerator>();

            services.AddTransient<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddTransient<IAccessTokenValidator, AccessTokenValidator>();

            return services;
        }
    }
}
