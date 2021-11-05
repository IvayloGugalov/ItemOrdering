using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(string firstName, string lastName, string email, string userName, string password, RoleToPermissions roleToPermissions);
    }
}