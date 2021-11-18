using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Identity.Domain.Interfaces;
using Identity.Infrastructure.MongoDB.Storages;
using Identity.Tokens.Interfaces;

namespace Identity.Infrastructure.MongoDB
{
    public static class StartupExtension
    {
        public static IServiceCollection AddMongoDbStorage(this IServiceCollection services)
        {

            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<IdentityDatabaseSettings>>().Value);

            services.AddSingleton<IMongoStorage, MongoStorage>();
            services.AddSingleton<IUserToRoleRepository, UsersToRolesStorage>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokensStorage>();
            services.AddSingleton<IRoleToPermissionRepository, RolesToPermissionsStorage>();

            services.AddTransient<IPermissionsSeeder, PermissionsSeeder>();

            return services;
        }
    }
}
