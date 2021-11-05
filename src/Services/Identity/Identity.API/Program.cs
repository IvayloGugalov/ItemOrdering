using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Identity.API.Extensions;
using Identity.Domain;
using Identity.Domain.Entities;

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
                var mongoSettings = services.GetRequiredService<IMongoDatabaseSettings>();
                var roleToPermissionCollection = MongoExtension.GetCollection<RoleToPermissions>(mongoSettings, mongoSettings.RolesToPermissionsCollectionName);

                if (await roleToPermissionCollection.EstimatedDocumentCountAsync() >= 0) return;

                var permissions = (Permissions[])Enum.GetValues(typeof(Permissions));

                foreach (var permission in permissions)
                {
                    var (name, description) = permission.GetAttributeInfo();
                    var packedPermission = permission.GetPermissionAsChar().ToString();

                    await roleToPermissionCollection.InsertOneAsync(new RoleToPermissions(name, description, packedPermission));
                }
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
