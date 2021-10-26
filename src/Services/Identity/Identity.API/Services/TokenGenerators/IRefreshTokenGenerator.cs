namespace Identity.API.Services.TokenGenerators
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}