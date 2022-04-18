using System;
using System.Collections.Generic;

using GuidGenerator;
using Microsoft.AspNetCore.Identity;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Functional.Tests.Proxies
{
    public class UserManagerProxy : IUserProxy
    {
        private readonly IUserToRoleRepository userToRoleRepository;
        private readonly UserManager<AuthUser> userManager;

        public UserManagerProxy(IUserToRoleRepository userToRoleRepository, UserManager<AuthUser> userManager)
        {
            this.userToRoleRepository = userToRoleRepository;
            this.userManager = userManager;
        }

        public AuthUser CreateUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            List<RoleToPermissions> roleToPermissions,
            IGuidGeneratorService guidGenerator)
        {
            try
            {
                var registrationUser = new AuthUser(
                    firstName: firstName,
                    lastName: lastName,
                    email: email,
                    userName: userName,
                    password: password,
                    roles: roleToPermissions,
                    guidGenerator);

                var result = this.userManager.CreateAsync(registrationUser, password).GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    foreach (var permission in roleToPermissions)
                    {
                        this.userToRoleRepository.CreateAsync(new UserToRole(registrationUser.Id, permission)).GetAwaiter().GetResult();
                    }
                }

                return registrationUser;
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
        AuthUser CreateUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            List<RoleToPermissions> roleToPermissions,
            IGuidGeneratorService guidGenerator);

        AuthUser GetUserByEmail(string email);
    }
}
