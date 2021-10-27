namespace Identity.Domain.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}
