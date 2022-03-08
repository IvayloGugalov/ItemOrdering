using System;
using System.Threading.Tasks;

namespace Identity.Tokens.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        Task<string> GenerateRefreshToken(Guid userId);
    }
}
