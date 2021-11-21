using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Identity.API;

namespace Identity.FunctionalTests
{
    public class TestIdentityWebAppFactory<TStartup> : WebApplicationFactory<Startup>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string TEST_SETTINGS_JSON = Path.Combine(
            Path.GetFullPath(
                Path.Combine("..", "..", "..", "..", "Identity.FunctionalTests")),
            "test_settings.json");


        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();
            
            // Get service provider.
            var serviceProvider = host.Services;

            host.Start();
            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // overwrite service registration

            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile(Path.GetFullPath(TEST_SETTINGS_JSON));
            });

            
        }
    }
}
