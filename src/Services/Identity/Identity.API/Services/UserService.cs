﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using GuidGenerator;

namespace Identity.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AuthUser> userManager;
        private readonly IUserToRoleRepository userToRoleRepository;
        private readonly IGuidGeneratorService guidGenerator;

        public UserService(UserManager<AuthUser> userManager, IUserToRoleRepository userToRoleRepository)
        {
            this.userManager = userManager;
            this.userToRoleRepository = userToRoleRepository;
            this.guidGenerator = new GuidGeneratorService();
        }

        public async Task<IdentityResult> RegisterUserAsync(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            RoleToPermissions roleToPermissions)
        {
            var registrationUser = new AuthUser(
                firstName: firstName,
                lastName: lastName,
                email: email,
                userName: userName,
                password: password,
                roles: new[] { roleToPermissions },
                this.guidGenerator);

            var result = await this.userManager.CreateAsync(registrationUser, password);

            if (result.Succeeded)
            {
                await this.userToRoleRepository.CreateAsync(new UserToRole(registrationUser.Id, roleToPermissions));
            }

            return result;
        }
    }
}
