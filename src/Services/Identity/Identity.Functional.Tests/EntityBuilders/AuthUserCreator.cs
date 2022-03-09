using System;

using Microsoft.Extensions.DependencyInjection;

using Identity.API;
using Identity.Domain.Entities;
using Identity.Functional.Tests.Proxies;

namespace Identity.Functional.Tests.EntityBuilders
{
    public static class AuthUserCreator
    {
        public static void Create(TestAuthUser testUser, TestIdentityWebAppFactory<Startup> factory)
        {
            using var scope = factory.Services.CreateScope();

            var userManagerProxy = scope.ServiceProvider.GetService<IUserProxy>();

            var permission = Enum.Parse<Permissions.Permissions>(testUser.Permissions);

            userManagerProxy.CreateUser(
                testUser.FirstName,
                testUser.LastName,
                testUser.Email,
                testUser.UserName,
                testUser.Password,
                new RoleToPermissions(permission));
        }
    }
}
