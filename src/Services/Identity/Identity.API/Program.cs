using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Identity.Infrastructure.MongoDB;

namespace Identity.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            await SeedDbWithGeneralPermissions(builder);

            await builder.RunAsync();
        }

        private static async Task SeedDbWithGeneralPermissions(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var permissionsSeeder = services.GetRequiredService<IPermissionsSeeder>();

                await permissionsSeeder.SeedDbWithPermissions();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
