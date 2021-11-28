using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Identity.API;
using Identity.Functional.Tests.Proxies;

namespace Identity.Functional.Tests
{
    public class TestIdentityWebAppFactory<TStartup> : WebApplicationFactory<Startup>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly string TEST_SETTINGS_JSON =
            Path.Combine(
                Path.GetFullPath(
                    Path.Combine("..", "..", "..", "..", "Identity.FunctionalTests")),
            "test_settings.json");


        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            // Get service provider.
            var serviceProvider = host.Services;
            
            // do stuff if needed

            host.Start();
            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // register required test services

            builder
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile(Path.GetFullPath(TEST_SETTINGS_JSON));
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient<IUserProxy, UserManagerProxy>();
                });
        }
    }
}
