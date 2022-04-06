using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Identity.Domain.Entities;
using Identity.Shared;

namespace Identity.Admin.Interfaces
{
    public interface IAdminUserService
    {
        IQueryable<AuthUser> QueryAuthUsersAsync();
        Task<AuthUser> FindAuthUserByIdAsync(Guid userId);
        Task<AuthUser> FindAuthUserByEmailAsync(string email);

        Task<ErrorResponse> AddNewUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<string> roleNames);

        Task<bool> DeleteAuthUserAsync(string email);
        Task<bool> UpdateUserAsync(Guid userId, string newUserName, string newEmail);
        Task<bool> UpdateUserRolesAsync(Guid userId, string roleName);
        Task<bool> RemoveRoleFromUser(Guid userId, string roleName);
    }
}
