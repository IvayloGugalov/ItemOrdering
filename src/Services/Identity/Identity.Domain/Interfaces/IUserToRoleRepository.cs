using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IUserToRoleRepository
    {
        Task<IEnumerable<UserToRole>> GetRolesForAuthUserAsync(Guid userId);
        Task<IEnumerable<UserToRole>> GetRolesAsync();
        Task CreateAsync(UserToRole userToRole);
        Task<bool> DeleteByUserIdAsync(Guid userId);
    }
}