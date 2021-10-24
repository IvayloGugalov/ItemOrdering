namespace ItemOrdering.Identity.API.Services.TokenGenerators
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}