using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GenericStatus;
using GuidGenerator;
using Microsoft.AspNetCore.Identity;

using Identity.Admin.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Admin.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRoleToPermissionRepository rolesToPermissionsRepository;
        private readonly IUserToRoleRepository usersToRolesRepository;
        private readonly UserManager<AuthUser> userManager;
        private readonly IGuidGeneratorService guidGenerator;

        public AdminUserService(
            IRoleToPermissionRepository rolesToPermissionsRepository,
            IUserToRoleRepository usersToRolesRepository,
            UserManager<AuthUser> userManager,
            IGuidGeneratorService guidGenerator)
        {
            this.rolesToPermissionsRepository = rolesToPermissionsRepository;
            this.usersToRolesRepository = usersToRolesRepository;
            this.userManager = userManager;
            this.guidGenerator = guidGenerator;
        }

        public IQueryable<AuthUser> QueryAuthUsersAsync()
        {
            return this.userManager.Users;
        }

        public async Task<AuthUser> FindAuthUserByIdAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            return user;
        }

        public async Task<AuthUser> FindAuthUserByEmailAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<IGenericStatus<AuthUser>> AddNewUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<string> roleNames)
        {
            var status = new GenericStatus<AuthUser>();
            var duplicateEmail = await this.userManager.FindByEmailAsync(email) != null;

            if (duplicateEmail) return status.AddError("Email already exists.");

            var roles = roleNames as string[] ?? roleNames.ToArray();
            var foundRoles = new List<RoleToPermissions>();
            if (roles.Any())
            {
                foundRoles = this.rolesToPermissionsRepository.QueryRoleToPermissions()
                    .Where(x => roles.Contains(x.DisplayName))
                    .ToList();
            }

            if (foundRoles.Count != roles.Length) return status.AddError("No such roles to add for user");

            var authUser = new AuthUser(
                firstName: firstName,
                lastName: lastName,
                email: email,
                userName: userName,
                password: password,
                roles: foundRoles,
                this.guidGenerator);

            var result = await this.userManager.CreateAsync(authUser, password);
            if (!result.Succeeded) return status.AddError(result.Errors.FirstOrDefault()?.ToString());

            // Map the role for the user inside the DB
            foundRoles.ForEach(async role =>
                await this.usersToRolesRepository.CreateAsync(new UserToRole(authUser.Id, role)));

            return status.SetResult(authUser);
        }

        public async Task<IGenericStatus<bool>> DeleteAuthUserAsync(string email)
        {
            var status = new GenericStatus<bool>();

            var user = await this.FindAuthUserByEmailAsync(email);
            if (user == null) return status.AddError("User with this email was not found.");

            var result = await this.userManager.DeleteAsync(user);
            return result.Succeeded
                ? status.SetResult(true)
                : status.AddError("Not able to delete user.");
        }

        public async Task<IGenericStatus<AuthUser>> UpdateUserAsync(Guid userId, string newUserName, string newEmail)
        {
            var status = new GenericStatus<AuthUser>();

            var authUser = await this.FindAuthUserByIdAsync(userId);
            if (authUser == null) return status.AddError("No user found.");

            authUser.UpdateUserNameAndEmail(newUserName, newEmail);

            return status;
        }

        public async Task<IGenericStatus<AuthUser>> UpdateUserRolesAsync(Guid userId, string roleName)
        {
            var result = new GenericStatus<AuthUser>();
            var status = await this.CheckUserAndRole(userId, roleName);
            if (status.HasErrors) return result.AddValidationResults(status.Errors);

            var (authUser, newRoleForUser) = status.Result;
            authUser.AddRoleToUser(newRoleForUser);

            return result.SetResult(authUser);
        }

        public async Task<IGenericStatus<AuthUser>> RemoveRoleFromUser(Guid userId, string roleName)
        {
            var result = new GenericStatus<AuthUser>();
            var status = await CheckUserAndRole(userId, roleName);
            if (status.HasErrors) return result.AddValidationResults(status.Errors);

            var (authUser, roleForUser) = status.Result;
            authUser.RemoveRoleFromUser(roleForUser);

            return result.SetResult(authUser);
        }

        private async Task<IGenericStatus<(AuthUser, RoleToPermissions)>> CheckUserAndRole(Guid userId, string roleName)
        {
            var status = new GenericStatus<(AuthUser, RoleToPermissions)>();

            var authUser = await this.FindAuthUserByIdAsync(userId);
            if (authUser == null) return status.AddError("No user found.");

            var roleForUser = await this.rolesToPermissionsRepository.GetByRoleNameAsync(roleName);
            return roleForUser == null
                ? status.AddError($"No such role {roleName} found.")
                : status.SetResult((authUser, roleForUser));
        }
    }
}
