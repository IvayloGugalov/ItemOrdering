using System;
using System.Threading.Tasks;

namespace Identity.Domain.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        Task<string> GenerateRefreshToken(Guid userId);
    }
}
