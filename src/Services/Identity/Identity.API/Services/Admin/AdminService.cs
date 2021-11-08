using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Identity.API.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AuthUser> userManager;
        private readonly IUserToRoleRepository userToRoleRepository;
        private readonly IMongoCollection<RoleToPermissions> rolesToPermissionsCollection;

        public AdminService(IMongoDatabaseSettings settings, IUserToRoleRepository userToRoleRepository, UserManager<AuthUser> userManager)
        {
            this.rolesToPermissionsCollection = MongoExtension.GetCollection<RoleToPermissions>(settings, settings.RolesToPermissionsCollectionName);
            this.userToRoleRepository = userToRoleRepository;
            this.userManager = userManager;
        }

        //public async Task<IEnumerable<AuthUser>> QueryAuthUsersAsync()
        //{
        //    return await this.usersCollection.Find(new BsonDocument()).ToListAsync();
        //}

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

        public async Task<ErrorResponse> AddNewUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<string> roleNames)
        {
            var duplicateEmail = await this.userManager.FindByEmailAsync(email) != null;

            // TODO: Create a separate error response?
            if (duplicateEmail) return new ErrorResponse("Email already exists.");

            var roles = roleNames as string[] ?? roleNames.ToArray();

            var foundRoles = new List<RoleToPermissions>();
            if (roles.Any())
            {
                foundRoles = (await this.rolesToPermissionsCollection.AsQueryable().ToListAsync())
                    .Where(x => roles.Contains(x.RoleName))
                    .ToList();
            }

            if (foundRoles.Count != roles.Length) throw new Exception("No such roles to add for user");

            var authUser = new AuthUser(
               firstName: firstName,
               lastName: lastName,
               email: email,
               userName: userName,
               password: password,
               roles: foundRoles);

            var result = await this.userManager.CreateAsync(authUser, password);

            if (!result.Succeeded) return result.GetErrorResponse();

            // Map the role for the user inside the DB
            foundRoles.ForEach(async role => await this.userToRoleRepository.CreateAsync(new UserToRole(authUser.Id, role)));

            return null;
        }

        public async Task<bool> DeleteAuthUserAsync(Guid userId)
        {
            var user = await this.FindAuthUserByIdAsync(userId);

            if (user == null) return false;

            var result = await this.userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        // TODO: Change Guid to AuthUser obj
        public async Task<bool> UpdateUserAsync(Guid userId, string newUserName, string newEmail)
        {
            var authUser = await this.FindAuthUserByIdAsync(userId);

            if (authUser == null) return false;

            authUser.UpdateUserNameAndEmail(newUserName, newEmail);

            return true;
        }

        // TODO: Change Guid to AuthUser obj
        public async Task<bool> UpdateUserRolesAsync(Guid userId, string roleName)
        {
            var authUser = await this.FindAuthUserByIdAsync(userId);

            if (authUser == null) return false;

            var newRoleForUser = await this.rolesToPermissionsCollection.Find(role => role.RoleName == roleName).SingleOrDefaultAsync();
            if (newRoleForUser == null) return false;

            authUser.AddRoleToUser(newRoleForUser);

            return true;
        }

        // TODO: Change Guid to AuthUser obj
        public async Task<bool> RemoveRoleFromUser(Guid userId, string roleName)
        {
            var authUser = await this.FindAuthUserByIdAsync(userId);

            if (authUser == null) return false;

            var roleForUser = await this.rolesToPermissionsCollection.Find(role => role.RoleName == roleName).SingleOrDefaultAsync();
            if (roleForUser == null) return false;

            authUser.RemoveRoleFromUser(roleForUser);

            return true;
        }

    }
}
