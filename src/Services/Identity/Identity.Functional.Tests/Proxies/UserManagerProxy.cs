using System;

using Microsoft.AspNetCore.Identity;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Functional.Tests.Proxies
{
    public class UserManagerProxy : IUserProxy
    {
        private readonly IUserService userService;
        private readonly UserManager<AuthUser> userManager;

        public UserManagerProxy(IUserService userService, UserManager<AuthUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public void CreateUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            RoleToPermissions roleToPermissions)
        {
            try
            {
                this.userService.RegisterUserAsync(
                        firstName,
                        lastName,
                        email,
                        userName,
                        password,
                        roleToPermissions)
                    .GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public AuthUser GetUserByEmail(string email)
        {
            return this.userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
        }
    }

    public interface IUserProxy
    {
        void CreateUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            RoleToPermissions roleToPermissions);

        AuthUser GetUserByEmail(string email);
    }
}
