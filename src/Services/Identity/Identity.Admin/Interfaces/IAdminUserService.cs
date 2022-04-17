using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GenericStatus;

using Identity.Domain.Entities;

namespace Identity.Admin.Interfaces
{
    public interface IAdminUserService
    {
        IQueryable<AuthUser> QueryAuthUsersAsync();
        Task<AuthUser> FindAuthUserByIdAsync(Guid userId);
        Task<AuthUser> FindAuthUserByEmailAsync(string email);

        Task<IGenericStatus<AuthUser>> AddNewUser(
            string firstName,
            string lastName,
            string email,
            string userName,
            string password,
            IEnumerable<string> roleNames);

        Task<IGenericStatus<bool>> DeleteAuthUserAsync(string email);
        Task<IGenericStatus<AuthUser>> UpdateUserAsync(Guid userId, string newUserName, string newEmail);
        Task<IGenericStatus<AuthUser>> UpdateUserRolesAsync(Guid userId, string roleName);
        Task<IGenericStatus<AuthUser>> RemoveRoleFromUser(Guid userId, string roleName);
    }
}
