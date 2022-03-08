using Identity.Admin.Interfaces;
using Identity.Admin.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Identity.Admin
{
    public static class StartupExtension
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IAdminRoleService, AdminRoleService>();

            return services;
        }
    }
}
