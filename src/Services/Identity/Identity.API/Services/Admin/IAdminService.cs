using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.API.Services.Admin
{
    public interface IAdminService
    {
        Task<AuthUser> FindAuthUserByIdAsync(Guid userId);
        Task<AuthUser> FindAuthUserByEmailAsync(string email);

        Task<ErrorResponse> AddNewUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<string> roleNames);

        Task<bool> DeleteAuthUserAsync(Guid userId);
        Task<bool> UpdateUserAsync(Guid userId, string newUserName, string newEmail);
        Task<bool> UpdateUserRolesAsync(Guid userId, string roleName);
        Task<bool> RemoveRoleFromUser(Guid userId, string roleName);
    }
}