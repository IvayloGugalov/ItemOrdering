using Microsoft.Extensions.DependencyInjection;

namespace GuidGenerator
{
    public static class StartUpExtension
    {
        public static IServiceCollection AddGuidGeneratorServices(this IServiceCollection services)
        {
            services.AddTransient<IGuidGeneratorService, GuidGeneratorService>();

            return services;
        }
    }
}
