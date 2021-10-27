using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Ordering.API;
using Ordering.Infrastructure.Data;

namespace Ordering.FunctionalTests
{
    public class TestWebAppFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            // Get service provider.
            var serviceProvider = host.Services;

            // Create a scope to obtain a reference to the database
            // context (AppDbContext).
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ItemOrderingDbContext>();

                var logger = scopedServices
                    .GetRequiredService<ILogger<TestWebAppFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    Seeder.Initialize(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                }
            }

            host.Start();
            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("src/Services/Ordering/Ordering.Api")
                .ConfigureServices(services =>
                {
                    // Remove the app's ItemOrderingDbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<ItemOrderingDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // This should be set for each individual test run
                    var inMemoryCollectionName = $"Test DB {Guid.NewGuid()}";

                    // Add ApplicationDbContext using an in-memory database for testing.
                    services.AddDbContext<ItemOrderingDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(inMemoryCollectionName);
                    });

                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerFactory>("Test", options => { });
                });
        }
    }
}
