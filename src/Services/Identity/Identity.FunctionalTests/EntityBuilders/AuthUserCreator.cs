using System;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using Identity.API;
using Identity.Domain.Entities;
using Identity.FunctionalTests.Proxies;
using Identity.Permissions;

namespace Identity.FunctionalTests.EntityBuilders
{
    public static class AuthUserCreator
    {
        public static void Create(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            Permissions.Permissions permission,
            TestIdentityWebAppFactory<Startup> factory)
        {
            // TODO: Remove this when all tests can run together without errors
            Thread.Sleep(TimeSpan.FromSeconds(1));
            using var scope = factory.Services.CreateScope();

            var userManagerProxy = scope.ServiceProvider.GetService<IUserProxy>();

            var (roleName, roleDescription) = permission.GetAttributeInfo();

            userManagerProxy.CreateUser(
                firstName,
                lastName,
                email,
                userName,
                password,
                new RoleToPermissions(
                    roleName,
                    roleDescription,
                    permission.GetPermissionAsChar().ToString()));
        }
    }
}
